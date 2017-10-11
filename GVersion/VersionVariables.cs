using System.Globalization;
using GVersionPluginInterface;
using LibGit2Sharp;
using Version = System.Version;

namespace GVersion
{
    public class VersionVariables : IVersionVariables
    {
        public VersionVariables(Version ver, IRepository repo, string preReleaseTag, string branchName)
        {
            Major = ver.Major.ToString(CultureInfo.InvariantCulture);
            Minor = ver.Minor.ToString(CultureInfo.InvariantCulture);
            Patch = ver.Build.ToString(CultureInfo.InvariantCulture);
            BuildMetaData = ver.Revision.ToString(CultureInfo.InvariantCulture);
            BuildMetaDataPadded = BuildMetaData.PadLeft(4, '0');
            BranchName = repo.Head.FriendlyName;
            if (!string.IsNullOrWhiteSpace(branchName))
            {
                BranchName = branchName;
            }
            PreReleaseTag = preReleaseTag;
            PreReleaseTagWithDash = $"-{PreReleaseTag}";
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
    }
}
