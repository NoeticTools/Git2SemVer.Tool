using NoeticTools.Git2SemVer.Core;
using NoeticTools.Git2SemVer.Core.Tools;


#pragma warning disable NUnit2045

namespace NoeticTools.Git2SemVer.Tool.Integration.Tests.Framework;

internal abstract class SolutionTestsBase : ScriptingTestsBase
{
    private string _currentDirectory = "";
    private string _git2SemVerToolPath = "";
    private string _solutionDirectory = "";
    protected string TestSolutionDirectory = "";
    protected string TestSolutionPath = "";
    protected string BuildConfiguration = "";

    protected abstract string SolutionFolderName { get; }

    protected abstract string SolutionName { get; }

    protected override void OneTimeSetUpBase()
    {
        base.OneTimeSetUpBase();
        _currentDirectory = Directory.GetCurrentDirectory();
        _solutionDirectory = DotNetProcessHelpers.GetSolutionDirectory();
        TestSolutionDirectory = Path.Combine(_solutionDirectory, "TestSolutions", SolutionFolderName);
        BuildConfiguration = new DirectoryInfo(_currentDirectory).Parent!.Name;
        TestSolutionPath = Path.Combine(TestSolutionDirectory, SolutionName);
        _git2SemVerToolPath =
            Path.Combine(_solutionDirectory, "Git2SemVer.Tool/bin", BuildConfiguration, "net8.0", "NoeticTools.Git2SemVer.Tool.dll");
    }

    protected static void DeleteAllNuGetPackages(string packageOutputDir)
    {
        if (string.IsNullOrWhiteSpace(packageOutputDir) || !Directory.Exists(packageOutputDir))
        {
            return;
        }

        foreach (var filePath in Directory.EnumerateFiles(packageOutputDir, "*.nupkg"))
        {
            File.Delete(filePath);
        }
    }

    protected string DeployScript(string scriptFilename)
    {
        var scriptPath = Path.Combine(TestFolderPath, scriptFilename);
        GetType().Assembly.WriteResourceFile(scriptFilename, scriptPath);
        return scriptPath;
    }

    protected (int returnCode, string stdOutput) ExecuteGit2SemVerTool(string commandLineArguments)
    {
        var process = new ProcessCli(Logger)
        {
            WorkingDirectory = TestSolutionDirectory
        };
        return process.Run("dotnet", $"{_git2SemVerToolPath} {commandLineArguments}");
    }
}