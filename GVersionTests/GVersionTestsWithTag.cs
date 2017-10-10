using System;
using NUnit.Framework;
using TestStack.BDDfy;

namespace GVersionTests
{
    [TestFixture]
    public class GVersionTestsWithTag : GVersionTestBase
    {
        [TestCase("1.0.1", "1.0.1.0")]
        [TestCase("1.0.1.10", "1.0.1.10")]
        public void WhenCurrentCommitIsTagged(string tag, string expectedVersion)
        {
            this.Given(t => ApplyTag(tag))
                .Then(t => VersionShouldBe(expectedVersion))
                .BDDfy();
        }

        [TestCase("1.0.1")]
        [TestCase("1.0.1.0")]
        public void WhenHasCommitsSinceLastTag(string tag)
        {
            this.Given(t => ApplyTag(tag))
                .And(t => MakeACommit("commit1"))
                .Then(t => VersionShouldBe("1.0.2.1"), "3rd number is bumpped")
                .BDDfy();
        }
    }
}
