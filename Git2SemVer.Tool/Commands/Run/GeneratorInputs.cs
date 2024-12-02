﻿using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Framework.Generation;


namespace NoeticTools.Git2SemVer.Tool.Commands.Run;

internal sealed class GeneratorInputs : IVersionGeneratorInputs
{
    public bool ValidateScriptInputs(ILogger logger)
    {
        return true;
    }

    public string BranchMaturityPattern { get; set; } = "";

    public string BuildContext { get; } = "";

    public string BuildIdFormat { get; } = "";

    public string BuildNumber { get; } = "";

    public string BuildScriptPath { get; set; } = "";

    public string HostType { get; set; } = "";

    public string IntermediateOutputDirectory { get; set; } = "";

    public bool? RunScript { get; set; }

    public string ScriptArgs { get; set; } = "";

    public string SolutionSharedDirectory { get; } = "";

    public string SolutionSharedVersioningPropsFile { get; } = "";

    public bool UpdateHostBuildLabel { get; };

    public string Version { get; } = "";

    public VersioningMode VersioningMode { get; set; } = VersioningMode.StandAloneProject;

    public string VersionSuffix { get; } = "";

    public string WorkingDirectory { get; } = "";
}