using System.Text.RegularExpressions;
using GVersionPluginInterface;
using LibGit2Sharp;
using Version = System.Version;

namespace GVersion.VersionStrategies
{
    public class BranchNameVersionStrategy : IVersionStrategy
    {
        public Version GetVersion(IRepository repo, Version knownHighestVersion)
        {
            var branchName = repo.Head.FriendlyName;
            var match = Regex.Match(branchName, @".*(\d+\.\d+\.\d+).*");
            if (!match.Success) return new Version(0, 0, 0, 0);
            var versionStr = match.Groups[1];
            return new Version(versionStr + ".0");
        }

        public string Name => nameof(BranchNameVersionStrategy);
        public int ExecutionOrder => 30000;
    }
}