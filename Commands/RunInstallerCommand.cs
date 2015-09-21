using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.CrossCutting.Logging.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace IwAutoUpdater.BLL.Commands
{
    public class RunInstallerCommand : Command
    {
        private readonly ILogger _logger;
        private readonly string _installerCommandArguments;
        private readonly string _installerCommand;
        private readonly string _fullPathToLocalDirectory;
        private readonly IUpdatePackage _package;
        private readonly string _workFolder;

        public RunInstallerCommand(string installerCommand, string installerCommandArguments, string workFolder, IUpdatePackage package, ILogger logger)
        {
            _workFolder = workFolder;
            _package = package;
            _logger = logger;

            _fullPathToLocalDirectory = Path.GetFileNameWithoutExtension(Path.Combine(_workFolder, package.Access.GetFilenameOnly()));

            _installerCommand = installerCommand;
            _installerCommandArguments = installerCommandArguments;
        }

        public override string PackageName
        {
            get
            {
                return _package.PackageName;
            }
        }

        public override CommandResult Do(CommandResult resultOfPreviousCommand)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe"; 
            p.StartInfo.Arguments = "/c " + _installerCommand + " " + _installerCommandArguments;
            p.StartInfo.WorkingDirectory = _fullPathToLocalDirectory;

            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;

            p.Start();

            var output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            var exitCode = p.ExitCode;

            _logger.Info(output);

            if (exitCode != 0)
            {
                return new CommandResult(false, new[] { new Error() { Text = output, Exception = null } });
            }
            else
            {
                return new CommandResult(true);
            }
        }
    }
}
