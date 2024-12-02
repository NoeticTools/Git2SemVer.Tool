﻿using NoeticTools.Git2SemVer.Tool.Commands.Add;
using NoeticTools.Git2SemVer.Tool.Commands.Remove;
using NoeticTools.Git2SemVer.Tool.Commands.Run;


namespace NoeticTools.Git2SemVer.Tool.Commands;

internal interface ICommandFactory
{
    ISetupCommand CreateAddCommand();

    IRemoveCommand CreateRemoveCommand();

    IRunCommand CreateRunCommand();
}