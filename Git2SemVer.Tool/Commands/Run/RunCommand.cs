using Injectio.Attributes;
using NoeticTools.Git2SemVer.Core.Exceptions;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Framework;
using NoeticTools.Git2SemVer.Framework.Generation;
using NoeticTools.Git2SemVer.Tool.Framework;
using System;
using NoeticTools.Git2SemVer.Framework.Persistence;
using NoeticTools.Git2SemVer.Tool.CommandLine;


namespace NoeticTools.Git2SemVer.Tool.Commands.Run;

[RegisterSingleton]
internal sealed class RunCommand : IRunCommand
{
    private readonly IConsoleIO _console;
    private readonly ILogger _logger;

    public RunCommand(IConsoleIO console,
                      ILogger logger)
    {
        _console = console;
        _logger = logger;
    }

    public bool HasError => _console.HasError;

    public void Execute(RunCommandSettings settings)
    {
        using var logger = new CompositeLogger();
        try
        {
            _console.WriteInfoLine($"Running Git2SemVer solution generator{(settings.Unattended ? " (unattended)" : "")}.");
            _console.WriteLine();

            var inputs = new GeneratorInputs
            {
                VersioningMode = VersioningMode.StandAloneProject,
                IntermediateOutputDirectory = settings.OutputDirectory
            };

            if (settings.HostType != null)
            {
                inputs.HostType = settings.HostType;
            }

            logger.Add(new NoDisposeLoggerDecorator(_logger));
            logger.Add(new ConsoleLogger());
            logger.Level = GetVerbosity(settings.Verbosity);

            IOutputsJsonIO outputJsonIO = settings.EnableJsonFileWrite ? 
                new OutputsJsonFileIO() : new ReadOnlyOutputJsonIO();
            var versionGenerator = new ProjectVersioningFactory(logger).Create(inputs, outputJsonIO);
            var outputs = versionGenerator.Run();

            _console.WriteLine($"""
                               
                               Outputs:
                               
                                  Assembly version:      {outputs.AssemblyVersion}
                                  File version:          {outputs.FileVersion}
                                  Package version:       {outputs.PackageVersion}
                                  Build system label:    {outputs.BuildSystemVersion}
                                  Informational version: {outputs.InformationalVersion}
                                  
                               Completed.
                               """);
        }
        catch (Exception exception)
        {
            _console.WriteErrorLine(exception);
            throw;
        }
    }

    private LoggingLevel GetVerbosity(string verbosity)
    {
        if (Enum.TryParse(value: verbosity, ignoreCase: true, out LoggingLevel level))
        {
            return level;
        }

        _console.WriteErrorLine($"Verbosity {verbosity} is not valid. Must be 'Trace', 'Debug', 'Info', 'Warning', or 'Error'.");
        return LoggingLevel.Info;
    }
}