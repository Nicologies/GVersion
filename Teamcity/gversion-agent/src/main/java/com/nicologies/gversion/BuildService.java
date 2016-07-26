package com.nicologies.gversion;

import com.nicologies.gversion.common.PathUtils;
import jetbrains.buildServer.RunBuildException;
import jetbrains.buildServer.agent.runner.BuildServiceAdapter;
import jetbrains.buildServer.agent.runner.ProgramCommandLine;
import jetbrains.buildServer.util.StringUtil;
import org.jetbrains.annotations.NotNull;

import java.io.File;
import java.net.URISyntaxException;
import java.util.List;
import java.util.Map;
import java.util.Vector;

public class BuildService extends BuildServiceAdapter {
    public BuildService() {
    }

    @NotNull
    public ProgramCommandLine makeProgramCommandLine() throws RunBuildException {
        return new ProgramCommandLine() {
            @NotNull
            public String getExecutablePath() throws RunBuildException {
                try {
                    return new File(PathUtils.GetExecutionPath(), "gversion.exe").getAbsolutePath();
                } catch (URISyntaxException e) {
                   throw new RunBuildException(e);
                }
            }

            @NotNull
            public String getWorkingDirectory() throws RunBuildException {
                return getCheckoutDirectory().getPath();
            }

            @NotNull
            public List<String> getArguments() throws RunBuildException {
                List<String> args =  new Vector<String>();
                args.add("-w");
                args.add(getCheckoutDirectory().getPath());
                args.add("-b");
                args.add(getBranchName());
                return args;
            }

            @NotNull
            public Map<String, String> getEnvironment() throws RunBuildException {
                return getBuildParameters().getEnvironmentVariables();
            }
        };
    }

    @NotNull
    private String getBranchName() {
        Map<String, String> configParams = getConfigParameters();
        String branchNameFromTeamcity = configParams.get("teamcity.build.branch");
        boolean isBuildingPullReqMerge = configParams.containsValue("refs/pull/" + branchNameFromTeamcity + "/merge");
        boolean isBuildingPullReqHead = configParams.containsValue("refs/pull/" + branchNameFromTeamcity + "/head");
        boolean isPullRequestBuild = isBuildingPullReqHead || isBuildingPullReqMerge;
        isPullRequestBuild &= StringUtil.isNumber(branchNameFromTeamcity);
        if (isPullRequestBuild) {
            if (isBuildingPullReqHead) {
                return "pull/" + branchNameFromTeamcity + "/head";
            }
            if (isBuildingPullReqMerge) {
                return "pull/" + branchNameFromTeamcity + "/merge";
            }
        }
        return branchNameFromTeamcity;
    }
}
