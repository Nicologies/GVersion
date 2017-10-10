using FluentAssertions;
using GVersion;
using LibGit2Sharp;
using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;

namespace GVersionTests
{
    public class GVersionTestBase
    {
        private const string ConfigFile = "GitVersionConfig.yaml";
        protected IRepository _repo;
        protected string _repoFolder;
        protected Signature _author = new Signature("user", "email@email.com", DateTime.Now);

        [SetUp]
        public void SetUp()
        {
            _repoFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "GVersionTests", Guid.NewGuid().ToString());
            Repository.Init(_repoFolder);
            _repo = new Repository(_repoFolder);

            _repo.Commit("init", _author, _author);
        }

        [TearDown]
        public void TearDown()
        {
            _repo.Dispose();
            RepoDeleter.Delete(_repoFolder);
        }

        protected void VersionShouldBe(string version)
        {
            var calculatedVersion = new VersionCalculator().GetVersion(_repoFolder, "master");
            calculatedVersion.ToString().Should().Be(version);
        }

        protected void ApplyTag(string tag)
        {
            _repo.ApplyTag(tag);
        }

        protected void MakeACommit(string commitMsg)
        {
            var file = Path.Combine(_repoFolder, Guid.NewGuid().ToString());
            File.WriteAllText(file, commitMsg);
            CommitFile(commitMsg, file);
        }

        private void CommitFile(string commitMsg, string file)
        {
            _repo.Stage(file);
            _repo.Commit(commitMsg, _author, _author);
        }

        protected void VersionInYmlIs(string version)
        {
            WriteToConfig($"next-version: {version}");
            string file = GetConfigFileFullPath();
            CommitFile($"Change Version in Yml to {version}", file);
        }

        protected void ToggleDateVersionStrategy(bool enabled)
        {
            WriteToConfig($"EnableDateVersionStrategy: {enabled}");
            string file = GetConfigFileFullPath();
            CommitFile($"Toggle EnableDateVersionStrategy in Yml to {enabled}", file);
        }

        private void WriteToConfig(string content)
        {
            string file = GetConfigFileFullPath();
            File.WriteAllText(file, content);
        }

        private string GetConfigFileFullPath()
        {
            return Path.Combine(_repoFolder, ConfigFile);
        }

        protected void BranchIs(string branch)
        {
            _repo.CreateBranch(branch);
            _repo.Checkout(branch);
        }
    }
}
