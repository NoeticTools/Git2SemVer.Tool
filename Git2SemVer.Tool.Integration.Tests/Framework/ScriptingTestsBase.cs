using System.Diagnostics;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Core.Tools;
using NoeticTools.Git2SemVer.Core.Tools.DotnetCli;
using NoeticTools.Git2SemVer.Core.Tools.Git;
using NoeticTools.Git2SemVer.Testing.Core;


namespace NoeticTools.Git2SemVer.Tool.Integration.Tests.Framework;

[NonParallelizable]
internal abstract class ScriptingTestsBase
{
    private const int MaximumTestDataFolders = 20;
    private static int _testDataFolderId; // avoid locks on folders not release quickly between tests

    private static bool WaitUntil(Func<bool> predicate)
    {
        var stopwatch = Stopwatch.StartNew();
        while (!predicate())
        {
            if (stopwatch.Elapsed > TimeSpan.FromSeconds(30))
            {
                return false;
            }

            Thread.Sleep(5);
        }

        return true;
    }

    protected string TestFolderPath = "";

    protected void SetUpBase()
    {
        Logger = new NUnitLogger { Level = LoggingLevel.Trace };

        if (_testDataFolderId > MaximumTestDataFolders)
        {
            _testDataFolderId = 0;
        }

        var dataFolderId = ++_testDataFolderId;
        TestFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                      "Git2SemVer",
                                      $"TestData{dataFolderId}");
        if (Directory.Exists(TestFolderPath))
        {
            Directory.Delete(TestFolderPath, true);
            if (!WaitUntil(() => !Directory.Exists(TestFolderPath)))
            {
                Assert.Fail($"Unable to deleted folder '{TestFolderPath}'.");
            }
        }

        Directory.CreateDirectory(TestFolderPath);
        if (!WaitUntil(() => Directory.Exists(TestFolderPath)))
        {
            Assert.Fail($"Unable to create folder '{TestFolderPath}'.");
        }
    }

    protected DotNetTool DotNetCli { get; private set; } = null!;

    protected ILogger Logger { get; private set; } = null!;

    protected virtual void OneTimeSetUpBase()
    {
        Logger = new NUnitLogger();
        DotNetCli = new DotNetTool(new ProcessCli(Logger));
        Git = new GitTool(Logger);
    }

    protected GitTool Git { get; private set; } = null!;
}