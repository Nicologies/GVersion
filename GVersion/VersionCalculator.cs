using GVersionPluginInterface;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GVersion
{
    public class VersionCalculator
    {
        [ImportMany(typeof(IVersionStrategy))]
        public IEnumerable<IVersionStrategy> VersionStrategies { get; set; }
        [ImportMany(typeof(IVersionOutput))]
        public IEnumerable<IVersionOutput> VersionOutputs { get; set; }
        public VersionCalculator()
        {
            ComposeParts();
        }
        private void ComposeParts()
        {
            var thisAssemblyCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());

            var pluginDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "GVersion", "plugins");
            if (!Directory.Exists(pluginDir))
            {
                Directory.CreateDirectory(pluginDir);
            }
            var directoryCatalog = new DirectoryCatalog(pluginDir);
            var pluginDirContainer = new CompositionContainer(directoryCatalog);

            var container = new CompositionContainer(thisAssemblyCatalog, pluginDirContainer);
            container.ComposeParts(this);
        }

        public System.Version GetVersion(string repoPath, string branchName)
        {
            if (!Repository.IsValid(repoPath))
            {
                Console.Error.WriteLine($"{repoPath} is not a valid repository");
                return null;
            }
            using (var repo = new Repository(repoPath))
            {
                var ver = new System.Version(0, 0, 0, 0);
                foreach (var strategy in VersionStrategies.OrderBy(r => r.ExecutionOrder))
                {
                    var newVer = strategy.GetVersion(repo, repoPath, ver);
                    if (newVer != null && newVer > ver)
                    {
                        ver = newVer;
                    }
                }

                foreach (var output in VersionOutputs)
                {
                    output.OutputVersion(
                        new VersionVariables(ver, repo, GetPreReleaseTag(repo, repoPath, branchName), branchName), repo);
                }
                return ver;
            }
        }


        private static string GetPreReleaseTag(IRepository repo, string repoPath, string branchName)
        {
            if (string.IsNullOrWhiteSpace(branchName))
            {
                branchName = repo.Head.FriendlyName;
            }
            var isFeatureBranch = Regex.Match(branchName, @"feature-.*", RegexOptions.IgnoreCase);
            if (isFeatureBranch.Success)
            {
                return "feature";
            }
            var isPullReq = Regex.Match(branchName, @".*pull/(\d+)/.*");
            if (isPullReq.Success)
            {
                return "PullRequest." + isPullReq.Groups[1];
            }

            var settings = new YamlConfigReader(repoPath).Settings;
            if (settings.TryGetValue("PreReleaseTag", out var prereleaseTag))
            {
                var strPrereleaseTag = prereleaseTag.ToString();
                return strPrereleaseTag.ToString().ToLowerInvariant() == "stable" ? "" : strPrereleaseTag;
            }
            
            var isReleaseBranch = Regex.Match(branchName, @"release-.*", RegexOptions.IgnoreCase);
            if (isReleaseBranch.Success)
            {
                return "";
            }
           
            var isHotfixBranch = repo.IsHotFixBranch();
            if (isHotfixBranch)
            {
                return "hotfix";
            }
            
            return branchName;
        }
    }
}
