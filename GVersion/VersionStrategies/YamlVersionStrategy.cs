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
                var tagVersionStrategy = new TagVersionStrategy();
                var tagVersion = tagVersionStrategy.GetVersion(repo, knownHighestVersion);
                return new Version(nextVersion.ToString() + "." + tagVersion.Revision);
            }
            return new Version(0,0,0,0);
        }

        public string Name => nameof(YamlVersionStrategy);
        public int ExecutionOrder => 50000;
    }
}