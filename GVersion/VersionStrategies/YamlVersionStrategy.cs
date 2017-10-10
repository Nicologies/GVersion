using GVersionPluginInterface;
using LibGit2Sharp;
using Version = System.Version;

namespace GVersion.VersionStrategies
{
    public class YamlVersionStrategy : IVersionStrategy
    {
        public Version GetVersion(IRepository repo, string repoFolder, Version knownHighestVersion)
        {
            if (repo.IsHeadCommitTagged())
            {
                return new Version(0, 0, 0, 0);
            }
            var settings = new YamlConfigReader(repoFolder).Settings;
            if (settings.TryGetValue("next-version", out object nextVersion))
            {
                var tagVersionStrategy = new TagVersionStrategy();
                var tagVersion = tagVersionStrategy.GetVersion(repo, repoFolder, knownHighestVersion);
                return new Version(nextVersion.ToString() + "." + tagVersion.Revision);
            }
            return new Version(0, 0, 0, 0);
        }

        public string Name => nameof(YamlVersionStrategy);
        public int ExecutionOrder => 50000;
    }
}