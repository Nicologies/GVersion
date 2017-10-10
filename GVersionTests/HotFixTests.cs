using NUnit.Framework;
using TestStack.BDDfy;

namespace GVersionTests
{
    [TestFixture]
    public class HotFixTests : GVersionTestBase
    {
        [TestCase("1.0.1", "1.0.1.1")]
        [TestCase("1.0.1.10", "1.0.1.11")]
        public void HotfixShouldNotIncreaseThe3rdNumber(string tag, string expected)
        {
            this.Given(t => ApplyTag(tag))
                .And(t => BranchIs("HotFix-1"))
                .And(t => MakeACommit("Commit1"))
                .Then(t => VersionShouldBe(expected))
                .BDDfy();
        }
    }
}
