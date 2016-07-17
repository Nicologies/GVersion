using System;
using GVersionPluginInterface;
using LibGit2Sharp;

namespace GVersion.VersionOutput
{
    public class ConsoleVersionOutput : IVersionOutput
    {
        public void OutputVersion(IVersionVariables ver, IRepository repo)
        {
            Console.WriteLine(ver.FullSemVer);
        }
    }
}
