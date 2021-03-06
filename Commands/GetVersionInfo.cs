﻿using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.DAL.Updates.Contracts;
using IwAutoUpdater.DAL.LocalFiles.Contracts;
using System.IO;
using IWAutoUpdater.CrossCutting.SFW.Contracts;

namespace IwAutoUpdater.BLL.Commands
{
    public class GetVersionInfo : Command
    {
        private readonly string _versionInfoFile;
        private readonly IBlackboard _blackboard;
        private readonly ISingleFile _singleFile;
        private readonly IUpdatePackage _package;
        private readonly string _workFolder;
        private readonly string _fullPathToLocalFile;

        public GetVersionInfo(string workFolder, IUpdatePackage package, string versionInfoFile, ISingleFile singleFile, IBlackboard blackboard)
        {
            _workFolder = workFolder;
            _package = package;
            _singleFile = singleFile;
            _blackboard = blackboard;
            _versionInfoFile = versionInfoFile;

            _fullPathToLocalFile = Path.Combine(_workFolder, Path.GetFileNameWithoutExtension(package.Access.GetFilenameOnly()), versionInfoFile);
        }

        public override CommandResult Do()
        {
            if (!_singleFile.DoesExist(_fullPathToLocalFile))
            {
                return new CommandResult(false, new[] { new Error() { Text = $"{_fullPathToLocalFile} does not exist", Exception = null } });
            }

            var content = _singleFile.ReadAsString(_fullPathToLocalFile);

            _blackboard.Add(_package.PackageName, new BlackboardEntry($"Update to Version: {content}"));

            return new CommandResult(true);
        }

        public override Command Copy()
        {
            var x = new GetVersionInfo(_workFolder, _package, _versionInfoFile, _singleFile, _blackboard);
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