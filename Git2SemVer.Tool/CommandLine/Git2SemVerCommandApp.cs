﻿using NoeticTools.Common.Logging;
using Spectre.Console.Cli;


namespace NoeticTools.Git2SemVer.Tool.CommandLine;

internal class Git2SemVerCommandApp
{
    public static int Execute(string[] args)
    {
        using var logger = new FileLogger(GetLogFilePath());
        var servicesProvider = Services.ConfigureServices(logger);
        var app = new CommandApp();

        app.Configure(config =>
        {
            config.SetApplicationName("Git2SemVer.Tool");
            config.UseAssemblyInformationalVersion();

            config.AddCommand<AddCliCommand>("add")
                  .WithDescription("Add Git2SemVer solution versioning to solution in working directory.")
                  .WithData(servicesProvider);
            config.AddCommand<RemoveCliCommand>("remove")
                  .WithDescription("Add Git2SemVer solution versioning to solution in working directory.")
                  .WithData(servicesProvider);
        });

        return app.Run(args);
    }

    private static string GetLogFilePath()
    {
        var folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Git2SemVer");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        return Path.Combine(folderPath, "Git2SemVer.Tool.log");
    }
}