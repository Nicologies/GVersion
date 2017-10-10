using System;
using NUnit.Framework;
using TestStack.BDDfy;

namespace GVersionTests
{
    [TestFixture]
    public class GVersionTestsWithTagAndYml : GVersionTestBase
    {
        [TestCase("1.0.1", "1.0.1.0")]
        [TestCase("1.0.1.10", "1.0.1.10")]
        public void WhenYmlVersionIsHigherThanTagAndCommitIsTagged(string tag, string expectedVersion)
        {
            this.Given(t => VersionInYmlIs("1.1.1"))
                .And(t => MakeACommit("Commit1"))
                .And(t => ApplyTag(tag))
                .Then(t => VersionShouldBe(expectedVersion))
                .BDDfy();
        }

        [TestCase("1.0.1", "1.1.1.1")]
        [TestCase("1.0.1.10", "1.1.1.11")]
        public void WhenYmlVersionIsHigherThanTagAndCommitIsNotTagged(string tag, string expectedVersion)
        {
            this.Given(t => VersionInYmlIs("1.1.1"))
                .And(t => MakeACommit("Commit1"))
                .And(t => ApplyTag(tag))
                .And(t => MakeACommit("Commit2"))
                .Then(t => VersionShouldBe(expectedVersion))
                .BDDfy();
        }
    }
}
