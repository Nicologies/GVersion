# GVersion

GVersion is a simplified/faster version of [GitVersion](https://github.com/GitTools/GitVersion) to help you achieve Semantic Versioning on your project.

It's designed for those who want a simple tool based on tags to generate a GitVersion-like version number without the full functionality of GitVersion.

By saying simplified, GVersion only checks the following version sources:

- Version numbers in current being built branch name (e.g. release/2.0.0)
- the `next-version` in `GitVersionConfig.yaml` file (e.g next-version: 2.0.0)
- the nearest tag (e.g. if the nearest tag is v2.0.0, then the version will 2.0.1)

By saying faster, GVersion usually takes 1 second to work out the version number while GitVersion may take a few minutes (even more than 1 hour in some edge case)

# Output Variables

## Teamcity

GVersion outputs the same variables as GitVersion does. Such as (not a full list of the variables):

 - GitVersion.AssemblySemVer  : 2.1.1.0
 - GitVersion.BranchName      : pull/105/merge
 - GitVersion.MajorMinorPatch : 2.1.1
 - GitVersion.BuildMetaData   : 85
 - GitVersion.FullSemVer      : 2.1.1-PullRequest.105+85

One difference is that GVersion will always output the `BuildMetaData` even if it's zero.

## Other build servers: not supported yet.

# Configuration

## Teamcity

no configuration required, the only thing you need is to add GVersion as a build step to your project.

You can change the `GitVersion` prefix of `GitVersion.FullSemVer` to something else by defining an env parameter "teamcity.GVersion.ParamPrefix"
