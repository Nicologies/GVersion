using LibGit2Sharp;
using System.Linq;

namespace GVersion
{
    internal static class RepositoryExtensions
    {
        public static bool IsHeadCommitTagged(this IRepository repo)
        {
            return repo.Tags.Any(x => x.PeeledTarget.Sha == repo.Head.Tip.Sha);
        }
    }
}
