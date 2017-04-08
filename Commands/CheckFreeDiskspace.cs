using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.DAL.Updates.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace IwAutoUpdater.BLL.Commands
{
    public class CheckFreeDiskspace : Command
    {
        private readonly string _fullPathToLocalFile;
        private readonly IUpdatePackage _package;
        private readonly string _workFolder;

        public CheckFreeDiskspace(string workFolder, IUpdatePackage package)
        {
            _workFolder = workFolder;
            _package = package;

            _fullPathToLocalFile = Path.Combine(_workFolder, package.Access.GetFilenameOnly());
        }

        public override CommandResult Do()
        {
            // das lokale Verzeichnis reicht, weil in der zip-Datei sowieso immer noch 
            // der passende Unterordner drin steckt
            var extractTo = Path.GetDirectoryName(_fullPathToLocalFile);
            ulong freebytes = 0;
            var success = DriveFreeBytes(extractTo, out freebytes);

            ulong minFreeBytes = (3ul * 1024 * 1024 * 1024); // 3 GB

            List<Error> errorsInThisCommand = new List<Error>();

            if (success && freebytes < minFreeBytes)
            {
                success = false;
                errorsInThisCommand.Add(new Error { Text = $"Required Diskspace: {minFreeBytes / 1024 / 1024 } MB, but free diskspace at '{extractTo}' is only {freebytes / 1024 / 1024 } MB" });
            }
            else if (!success)
            {
                errorsInThisCommand.Add(new Error { Text = $"Getting free diskspace at '{extractTo}' failed" });
            }

            return new CommandResult(success, errorsInThisCommand);
        }

        public override Command Copy()
        {
            var x = new CheckFreeDiskspace(_workFolder, _package);
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

        // Pinvoke for API function
        // http://stackoverflow.com/a/13578940
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
        out ulong lpFreeBytesAvailable,
        out ulong lpTotalNumberOfBytes,
        out ulong lpTotalNumberOfFreeBytes);

        private static bool DriveFreeBytes(string folderName, out ulong freespace)
        {
            freespace = 0;
            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentNullException("folderName");
            }

            if (!folderName.EndsWith("\\"))
            {
                folderName += '\\';
            }

            ulong free = 0, dummy1 = 0, dummy2 = 0;

            if (GetDiskFreeSpaceEx(folderName, out free, out dummy1, out dummy2))
            {
                freespace = free;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
