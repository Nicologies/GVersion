namespace GVersionPluginInterface
{
    public interface IVersionVariables
    {
        string Major { get; }

        string Minor { get; }

        string Patch { get; }

        string PreReleaseTag { get; }

        string PreReleaseTagWithDash { get; }

        string BuildMetaData { get; }

        string BuildMetaDataPadded { get; }

        string FullBuildMetaData { get; }

        string MajorMinorPatch { get; }

        string SemVer { get; }

        string AssemblySemVer { get; }

        string FullSemVer { get; }

        string InformationalVersion { get; }

        string BranchName { get; }

        string Sha { get; }

        string CommitsSinceVersionSource { get; }

        string CommitsSinceVersionSourcePadded { get; }

        string CommitDate { get; }
    }
}
