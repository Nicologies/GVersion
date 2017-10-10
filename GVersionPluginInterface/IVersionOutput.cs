using System.ComponentModel.Composition;
using LibGit2Sharp;

namespace GVersionPluginInterface
{
    [InheritedExport]
    public interface IVersionOutput
    {
        void OutputVersion(IVersionVariables ver, IRepository repo);
    }
}
