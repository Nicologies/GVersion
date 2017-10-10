using System.ComponentModel.Composition;
using LibGit2Sharp;
using Version = System.Version;

namespace GVersionPluginInterface
{
    [InheritedExport]
    public interface IVersionStrategy
    {
        Version GetVersion(IRepository repo, string repoFolder, Version knownHighestVersion);
        string Name { get; }
        int ExecutionOrder { get; }
    }
}