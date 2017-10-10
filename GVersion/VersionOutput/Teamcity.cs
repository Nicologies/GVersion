using System;
using GVersionPluginInterface;
using LibGit2Sharp;

namespace GVersion.VersionOutput
{
    public class Teamcity : IVersionOutput
    {
        private const string KeyOfTeamcityGVersionParamPrefix = "Teamcity.GVersion.ParamPrefix";
        private string ParamPrefix { get; } = "GitVersion";

        public Teamcity()
        {
            var fromEnv = Environment.GetEnvironmentVariable(KeyOfTeamcityGVersionParamPrefix);
            if (!string.IsNullOrWhiteSpace(fromEnv))
            {
                ParamPrefix = fromEnv.Trim();
            }
        }

        public void OutputVersion(IVersionVariables ver, IRepository repo)
        {
            foreach (var prop in ver.GetType().GetProperties())
            {
                var name = prop.Name;
                var value = prop.GetValue(ver).ToString();
                Console.WriteLine($"##teamcity[setParameter name='{ParamPrefix}.{name}'" +
                              $" value='{EscapeValue(value)}']");
                Console.WriteLine($"##teamcity[setParameter name='system.{ParamPrefix}.{name}'" +
                              $" value='{EscapeValue(value)}']");
            }
            Console.WriteLine($"##teamcity[buildNumber '{EscapeValue(ver.FullSemVer)}']");
        }

        public static string EscapeValue(string value)
        {
            if (value == null)
            {
                return null;
            }
            // List of escape values from http://confluence.jetbrains.com/display/TCD8/Build+Script+Interaction+with+TeamCity

            value = value.Replace("|", "||");
            value = value.Replace("'", "|'");
            value = value.Replace("[", "|[");
            value = value.Replace("]", "|]");
            value = value.Replace("\r", "|r");
            value = value.Replace("\n", "|n");

            return value;
        }
    }
}
