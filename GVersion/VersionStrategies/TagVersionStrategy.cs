﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using GVersionPluginInterface;
using LibGit2Sharp;
using Version = System.Version;

namespace GVersion.VersionStrategies
{
    public class TagVersionStrategy : IVersionStrategy
    {
        public Version GetVersion(IRepository repo, Version knownHighestVersion)
        {
            try
            {
                var versionStr = repo.Describe(repo.Head.Tip, new DescribeOptions()
                {
                    Strategy = DescribeStrategy.Tags,
                    AlwaysRenderLongFormat = true
                });
                var match = Regex.Match(versionStr, @".*?((\d+\.){2,3}\d+\-\d+).*");
                if (!match.Success) return new Version(0, 0, 0, 0);
                versionStr = string.Join(".",
                    match.Groups[1].ToString().Split(new []{'.', '-'}, StringSplitOptions.RemoveEmptyEntries).Take(4));
                var curVer = new Version(versionStr);
                var isHeadCommitTagged = repo.Tags.Any(x => x.PeeledTarget.Sha == repo.Head.Tip.Sha);
                var nextVer = new Version(curVer.Major, curVer.Minor,
                   isHeadCommitTagged? curVer.Build : curVer.Build + 1, curVer.Revision);
                return nextVer;
            }
            catch
            {
                return new Version(0, 0, 0, 0);
            }
        }

        public string Name => nameof(TagVersionStrategy);
        public int ExecutionOrder => 90000;
    }
}