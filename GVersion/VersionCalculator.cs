using GVersionPluginInterface;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;

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
                        new VersionVariables(ver, repo, branchName), repo);
                }
                return ver;
            }
        }
    }
}
