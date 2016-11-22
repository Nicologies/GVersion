using System.Text.RegularExpressions;
using GVersionPluginInterface;
using LibGit2Sharp;
using Version = System.Version;

namespace GVersion.VersionStrategies
{
    public class YamlVersionStrategy : IVersionStrategy
    {
        public Version GetVersion(IRepository repo, Version knownHighestVersion)
        {
            var settings = SettingsProvider.Instance.Settings;
            object nextVersion;
            if (settings.TryGetValue("next-version", out nextVersion))
            {
                var versionStr = repo.Describe(repo.Head.Tip, new DescribeOptions()
                {
                    Strategy = DescribeStrategy.Tags,
                    AlwaysRenderLongFormat = true
                });
                var match = Regex.Match(versionStr, @".*-(\d*)-.*");
                var commitsSinceLastTag = match.Groups[1].Value;
                return new Version(nextVersion.ToString() + "." + commitsSinceLastTag);
            }
            return new Version(0,0,0,0);
        }

        public string Name => nameof(YamlVersionStrategy);
        public int ExecutionOrder => 50000;
    }
}