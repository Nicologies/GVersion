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
                return new Version(nextVersion.ToString() + ".0");
            }
            return new Version(0,0,0,0);
        }

        public string Name => nameof(YamlVersionStrategy);
        public int ExecutionOrder => 50000;
    }
}