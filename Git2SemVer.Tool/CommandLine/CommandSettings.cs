using System.ComponentModel;
using Spectre.Console.Cli;
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable ClassNeverInstantiated.Global


namespace NoeticTools.Git2SemVer.Tool.CommandLine;

public class CommonCommandSettings : CommandSettings
{
    [CommandOption("-u|--unattended")]
    [DefaultValue(false)]
    [Description("Unattened execution. Accepts all defaults.")]
    public bool Unattended { get; set; }
}

public class SolutionCommandSettings : CommonCommandSettings
{
    [CommandOption("-s|--solution")]
    [DefaultValue("")]
    [Description("Solution name. Optional, only required when there are multiple solutions in the working directory.")]
    public string SolutionName { get; set; } = "";
}

public class RunCommandSettings : CommonCommandSettings
{
    [CommandOption("-o|--output")]
    [DefaultValue("")]
    [Description("Directory in which to place the generated version JSON file and the build log.")]
    public string OutputDirectory { get; set; } = "";

    [CommandOption("--enable-json-write")]
    [DefaultValue(false)]
    [Description("Enables writing of generated version JSON file.")]
    public bool EnableJsonFileWrite { get; set; }

    [CommandOption("--show-build-log")]
    [DefaultValue(false)]
    [Description("Enables showing version generator's build log.")]
    public bool ShowBuildLog { get; set; }

    [CommandOption("-v|--verbosity <level>")]
    [DefaultValue("info")]
    
    [Description("Sets logging verbosity and, to a limited extent, console output.")]
    public string Verbosity { get; set; } = "";
}