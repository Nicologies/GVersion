using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CommandLine;
using CommandLine.Text;
using GVersionPluginInterface;
using LibGit2Sharp;
using Version = System.Version;

namespace GVersion
{
    class Options
    {
        [Option('w', "workingdir", Required = false,
            HelpText = "Working directory.")]
        public string WorkingDir { get; set; } = ".";
        [Option('b', "label", Required = false,
                    HelpText = "The semver label for example the 'PullRequest.2500' in '1.10.1-PullRequest.2500+199'," +
                               "or branch name.")]
        public string Label { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              x => HelpText.DefaultParsingErrorsHandler(this, x));
        }
    }
    class Program
    {
        [ImportMany(typeof(IVersionStrategy))]
        public IEnumerable<IVersionStrategy> VersionStrategies { get; set; }
        [ImportMany(typeof(IVersionOutput))]
        public IEnumerable<IVersionOutput> VersionOutputs { get; set; }

        Program()
        {
            ComposeParts();
        }

        static void Main(string[] args)
        {
            var options = new Options();
            if (!Parser.Default.ParseArguments(args, options))
            {
                Environment.Exit(-1);
            }
            var prog = new Program();
            prog.ComposeParts();
            Environment.Exit(prog.GetVersion(options.WorkingDir, options.Label) ? 0 : 1);
        }

        private bool GetVersion(string repoPath, string label)
        {
            if (!Repository.IsValid(repoPath))
            {
                Console.Error.WriteLine($"{repoPath} is not a valid repository");
                return false;
            }
            using (var repo = new Repository(repoPath))
            {
                var ver = new Version(0, 0, 0, 0);
                foreach (var strategy in VersionStrategies.OrderBy(r => r.ExecutionOrder))
                {
                    var newVer = strategy.GetVersion(repo, ver);
                    if (newVer != null && newVer > ver)
                    {
                        ver = newVer;
                    }
                }

                foreach (var output in VersionOutputs)
                {
                    output.OutputVersion(new VersionVariables(ver, repo, label), repo);
                }
            }
            return true;
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
    }
}
