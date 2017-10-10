using LibGit2Sharp;
using System.Linq;
using System.Text.RegularExpressions;

namespace GVersion
{
    internal static class RepositoryExtensions
    {
        public static bool IsHeadCommitTagged(this IRepository repo)
        {
            return repo.Tags.Any(x => x.PeeledTarget.Sha == repo.Head.Tip.Sha);
        }
        public static bool IsHotFixBranch(this IRepository repo)
        {
            var branchName = repo.Head.FriendlyName;
            var match = Regex.Match(branchName, @"hotfix-.*", RegexOptions.IgnoreCase);
            return match.Success;
        }
    }
}
