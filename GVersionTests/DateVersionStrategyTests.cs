using System;
using NUnit.Framework;
using TestStack.BDDfy;

namespace GVersionTests
{
    [TestFixture]
    public class DateVersionStrategyTests : GVersionTestBase
    {
        [Test]
        public void WhenDateVersionStrategyIsDisabled()
        {
            this.Given(t => DateVersionStrategyIsDisabled())
                .And(t => MakeACommit("commit1"))
                .And(t => ApplyTag("1.0.0"))
                .And(t => MakeACommit("commit2"))
                .Then(t => VersionShouldBe("1.0.1.1"))
                .BDDfy();
        }

        [Test]
        public void WhenDateVersionStrategyIsEnabled()
        {
            this.Given(t => DateVersionStrategyIsEnabled())
                .And(t => MakeACommit("commit1"))
                .And(t => ApplyTag("1.0.0"))
                .And(t => MakeACommit("commit2"))
                .Then(t => VersionShouldBe($"{DateTime.Today.Year}.{DateTime.Today.Month}.1.1"))
                .BDDfy();
        }

        [TestCase("1.0.1", "1.0.1.1")]
        [TestCase("1.0.1.10", "1.0.1.11")]
        public void DateVersionStrategyShouldNotApplyOnHotFix(string tag, string expected)
        {
            this.Given(t => DateVersionStrategyIsEnabled())
                .And(t => ApplyTag(tag))
                .And(t => BranchIs("HotFix-1"))
                .And(t => MakeACommit("Commit1"))
                .Then(t => VersionShouldBe(expected))
                .BDDfy();
        }

        private void DateVersionStrategyIsDisabled()
        {
            ToggleDateVersionStrategy(enabled: false);
        }

        private void DateVersionStrategyIsEnabled()
        {
            ToggleDateVersionStrategy(enabled: true);
        }
    }
}
