using System.Globalization;
using System.Text.RegularExpressions;
using GVersionPluginInterface;
using LibGit2Sharp;
using Version = System.Version;

namespace GVersion
{
    public class VersionVariables : IVersionVariables
    {
        public VersionVariables(Version ver, IRepository repo, string branchName)
        {
            Major = ver.Major.ToString(CultureInfo.InvariantCulture);
            Minor = ver.Minor.ToString(CultureInfo.InvariantCulture);
            Patch = ver.Build.ToString(CultureInfo.InvariantCulture);
            BuildMetaData = ver.Revision.ToString(CultureInfo.InvariantCulture);
            BuildMetaDataPadded = BuildMetaData.PadLeft(4, '0');
            if (string.IsNullOrWhiteSpace(branchName))
            {
                branchName = repo.Head.FriendlyName;
            }
            var label = GetBranchLabel(branchName);
            PreReleaseTag = label; 
            PreReleaseTagWithDash = $"-{PreReleaseTag}";
            BranchName = branchName;
            FullBuildMetaData = $"{ver.Revision}.Branch.{BranchName}.Sha.{repo.Head.Tip.Sha}";
            MajorMinorPatch = $"{ver.Major}.{ver.Minor}.{ver.Build}";
            SemVer = $"{ver.Major}.{ver.Minor}.{ver.Build}-{PreReleaseTag}";
            AssemblySemVer = $"{ver.Major}.{ver.Minor}.{ver.Build}.0";
            FullSemVer = $"{SemVer}+{ver.Revision}";
            Sha = repo.Head.Tip.Sha;
            InformationalVersion = $"{FullSemVer}.Branch.{BranchName}.Sha.{Sha}";
            CommitsSinceVersionSource = ver.Revision.ToString(CultureInfo.InvariantCulture);
            CommitsSinceVersionSourcePadded = CommitsSinceVersionSource.PadLeft(4, '0');
            CommitDate = repo.Head.Tip.Committer.When.Date.ToString("yyyy-MM-dd");
        }
        public string Major { get; }

        public string Minor { get; }

        public string Patch { get; }

        public string PreReleaseTag { get; }

        public string PreReleaseTagWithDash { get; }

        public string BuildMetaData { get; }

        public string BuildMetaDataPadded { get; }

        public string FullBuildMetaData { get; }

        public string MajorMinorPatch { get; }

        public string SemVer { get; }

        public string AssemblySemVer { get; }

        public string FullSemVer { get; }

        public string InformationalVersion { get; }

        public string BranchName { get; }

        public string Sha { get; }

        public string CommitsSinceVersionSource { get; }

        public string CommitsSinceVersionSourcePadded { get; }

        public string CommitDate { get; }

        private static string GetBranchLabel(string branchName)
        {
            if (string.IsNullOrWhiteSpace(branchName))
            {
                return null;
            }
            var isReleaseBranch = Regex.Match(branchName, @"release-.*\d\.\d\.\d.*", RegexOptions.IgnoreCase);
            if (isReleaseBranch.Success)
            {
                return "beta";
            }
            var isHotfixBranch = Regex.Match(branchName, @"hotfix-.*\d\.\d\.\d.*", RegexOptions.IgnoreCase);
            if (isHotfixBranch.Success)
            {
                return "beta";
            }
            var isPullReq = Regex.Match(branchName, @"pull/(\d+)/.*");
            if (isPullReq.Success)
            {
                return "PullRequest." + isPullReq.Groups[1];
            }
            return branchName;
        }
    }
}
