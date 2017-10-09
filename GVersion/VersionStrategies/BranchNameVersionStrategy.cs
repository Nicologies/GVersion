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
            var match = Regex.Match(branchName, @".*?((\d+\.){2,3}\d+).*");
            if (!match.Success) return new Version(0, 0, 0, 0);
            var versionStr = match.Groups[1];
            var ret = new Version(versionStr.Value);
            if (ret.Revision == -1)
            {
                ret = new Version(ret.Major, ret.Minor, ret.Build, 0);
            }
            return ret;
        }

        public string Name => nameof(BranchNameVersionStrategy);
        public int ExecutionOrder => 30000;
    }
}