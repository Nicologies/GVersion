using System;
using CommandLine;
using CommandLine.Text;

namespace GVersion
{
    class Options
    {
        [Option('w', "workingdir", Required = false,
            HelpText = "Working directory.")]
        public string WorkingDir { get; set; } = ".";
        [Option('b', "branch", Required = false,
                    HelpText = "Branch name or pull/{pull req num}/merge or pull/{pull req num}/head.\n" +
                               "Current branch will be used if not specified.")]
        public string Branch { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              x => HelpText.DefaultParsingErrorsHandler(this, x));
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (!Parser.Default.ParseArguments(args, options))
            {
                Environment.Exit(-1);
            }
            var calculator = new VersionCalculator();
            Environment.Exit(calculator.GetVersion(options.WorkingDir, options.Branch) != null ? 0 : 1);
        }
    }
}
