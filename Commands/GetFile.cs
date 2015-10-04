﻿using System;
using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.DAL.LocalFiles.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using System.IO;

namespace IwAutoUpdater.BLL.Commands
{
    public class GetFile : Command
    {
        private readonly IUpdatePackage _package;
        private readonly ISingleFile _singleFile;
        private readonly string _workFolder;
        private readonly string _fullPathToLocalFile;

        public GetFile(string workFolder, IUpdatePackage package, ISingleFile singleFile)
        {
            _workFolder = workFolder;
            _package = package;
            _singleFile = singleFile;

            _fullPathToLocalFile = Path.Combine(_workFolder, package.Access.GetFilenameOnly());
        }

        public override CommandResult Do(CommandResult lastResult)
        {
            var remoteFile = _package.Access.GetFile();
            var writeSuccess = _singleFile.Write(_fullPathToLocalFile, remoteFile);
            return new CommandResult(writeSuccess);
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