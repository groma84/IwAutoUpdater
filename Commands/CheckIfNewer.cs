﻿using System;
using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.DAL.Updates.Contracts;
using IwAutoUpdater.DAL.LocalFiles.Contracts;
using System.IO;

namespace IwAutoUpdater.BLL.Commands
{
    public class CheckIfNewer : Command
    {
        private readonly ISingleFile _singleFile;
        private readonly IUpdatePackage _package;
        private readonly string _workFolder;
        private readonly string _fullPathToLocalFile;

        public CheckIfNewer(string workFolder, IUpdatePackage package, ISingleFile singleFile)
        {
            _workFolder = workFolder;
            _package = package;
            _singleFile = singleFile;

            _fullPathToLocalFile = Path.Combine(_workFolder, package.Access.GetFilenameOnly());

            _vordraengelFaktor = int.MaxValue; // CheckIfNewer aller Pakete soll immer mit maximaler Priorität ausgeführt werden
        }

        public override CommandResult Do()
        {
            var localFileDate = (_singleFile.DoesExist(_fullPathToLocalFile) ? _singleFile.GetLastModified(_fullPathToLocalFile) : DateTime.MinValue);
            var remoteIsNewer = _package.Access.IsRemoteFileNewer(localFileDate);
            return new CommandResult(remoteIsNewer);
        }

        public override Command Copy()
        {
            var x = new CheckIfNewer(_workFolder, _package, _singleFile);
            x.RunAfterCompletedWithResultFalse = this.RunAfterCompletedWithResultFalse;
            x.RunAfterCompletedWithResultTrue = this.RunAfterCompletedWithResultTrue;
            x.AddResultsOfPreviousCommands(this.ResultsOfPreviousCommands);

            return x;
        }

        public override string PackageName
        {
            get
            {
                return _package.PackageName;
            }
        }
    }
}