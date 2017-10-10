using LibGit2Sharp;
using System;

namespace GVersion.VersionStrategies
{
    public class DateVersionStrategy : GVersionPluginInterface.IVersionStrategy
    {
        public string Name => nameof(DateVersionStrategy);

        public int ExecutionOrder => 999999999;

        public System.Version GetVersion(IRepository repo, string repoFolder, System.Version knownHighestVersion)
        {
            if (repo.IsHeadCommitTagged())
            {
                return new System.Version(0, 0, 0, 0);
            }
            if (repo.IsHotFixBranch())
            {
                return new System.Version(0, 0, 0, 0);
            }
            var settingsReader = new YamlConfigReader(repoFolder);
            if (settingsReader.Settings.TryGetValue("EnableDateVersionStrategy", out var enabled))
            {
                if (enabled.ToString().ToLowerInvariant() == "true")
                {
                    var tagVersionStrategy = new TagVersionStrategy();
                    var tagVersion = tagVersionStrategy.GetVersion(repo, repoFolder, knownHighestVersion);
                    var major = DateTime.Today.Year;
                    var minor = DateTime.Today.Month;
                    return new System.Version(major, minor, tagVersion.Build, tagVersion.Revision);
                }
            }
            return new System.Version(0, 0, 0, 0);
        }
    }
}
