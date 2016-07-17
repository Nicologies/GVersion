# GVersion

GVersion is a simplified/faster version of [GitVersion](https://github.com/GitTools/GitVersion) to help you achieve Semantic Versioning on your project
It's designed for those who want a simple tool to generate a GitVersion-like version number without the full functionality of GitVersion.

By saying simplified, GVersion only checks the following version sources:

- Version numbers in branches (e.g. release/2.0.0)
- the `next-version` in `GitVersionConfig.yaml` file (e.g next-version: 2.0.0)
- the nearest tag (e.g. if the nearest tag is v2.0.0, then the version will 2.0.1)

# Output Variables

## Teamcity

GVersion outputs the same variables as GitVersion does. Such as (not a full list of the variables):

 - GitVersion.AssemblySemVer  : 2.1.1.0
 - GitVersion.BranchName      : PullRequest.105
 - GitVersion.MajorMinorPatch : 2.1.1
 - GitVersion.BuildMetaData   : 85
 - GitVersion.FullSemVer      : 2.1.1-PullRequest.105+85

## Other build servers: not supported yet.

# Configuration

## Teamcity

no configuration required, the only thing you need is to add GVersion as a build step to your project.

You can change the `GitVersion` prefix of `GitVersion.FullSemVer` to something else by defining an env parameter "teamcity.GVersion.ParamPrefix"

## Other build servers: not supported yet.

# Collabrate with [PrExtras](https://github.com/Nicologies/PrExtras)

GVersion is designed to be able to read the branch name of pull request that retrieved by [PrExtras](https://github.com/Nicologies/PrExtras); thereby displaying the real branch name in the semver instead of the pull request number.
