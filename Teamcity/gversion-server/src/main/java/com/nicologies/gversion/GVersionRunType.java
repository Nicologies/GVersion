package com.nicologies.gversion;

import jetbrains.buildServer.serverSide.PropertiesProcessor;
import jetbrains.buildServer.serverSide.RunTypeRegistry;
import org.jetbrains.annotations.NotNull;
import org.jetbrains.annotations.Nullable;

import java.util.Map;


public class GVersionRunType extends jetbrains.buildServer.serverSide.RunType{
    public GVersionRunType(final RunTypeRegistry runTypeRegistry){
        runTypeRegistry.registerRunType(this);
    }
    @NotNull
    @Override
    public String getType() {
        return com.nicologies.gversion.common.GVersionConstants.RunnerType;
    }

    @NotNull
    @Override
    public String getDisplayName() {
        return "GVersion";
    }

    @NotNull
    @Override
    public String getDescription() {
        return "Looks at your git history and works out the semantic version (semver.org) of the commit being built.";
    }

    @Nullable
    @Override
    public PropertiesProcessor getRunnerPropertiesProcessor() {
        return null;
    }

    @Nullable
    @Override
    public String getEditRunnerParamsJspFilePath() {
        return null;
    }

    @Nullable
    @Override
    public String getViewRunnerParamsJspFilePath() {
        return null;
    }

    @Nullable
    @Override
    public Map<String, String> getDefaultRunnerProperties() {
        return null;
    }
}
