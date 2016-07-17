package com.nicologies.gversion;

import com.nicologies.gversion.common.GVersionConstants;
import jetbrains.buildServer.agent.AgentBuildRunnerInfo;
import jetbrains.buildServer.agent.BuildAgentConfiguration;
import jetbrains.buildServer.agent.runner.CommandLineBuildService;
import jetbrains.buildServer.agent.runner.CommandLineBuildServiceFactory;
import org.apache.log4j.Logger;
import org.jetbrains.annotations.NotNull;

public class BuildServiceFactory implements CommandLineBuildServiceFactory, AgentBuildRunnerInfo {
    private static final Logger LOG = Logger.getLogger(BuildServiceFactory.class);

    public BuildServiceFactory() {
    }

    @NotNull
    public String getType() {
        return GVersionConstants.RunnerType;
    }

    public boolean canRun(@NotNull final BuildAgentConfiguration agentConfiguration) {
        return true;
    }


    @NotNull
    public CommandLineBuildService createService() {
        return new BuildService();
    }

    @NotNull
    public AgentBuildRunnerInfo getBuildRunnerInfo() {
        return this;
    }
}