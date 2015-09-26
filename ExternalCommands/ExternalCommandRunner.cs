using IwAutoUpdater.DAL.ExternalCommands.Contracts;
using System.Diagnostics;

namespace IwAutoUpdater.DAL.ExternalCommands
{
    public class ExternalCommandRunner : IRunExternalCommand
    {
        ExternalCommandResult IRunExternalCommand.Run(string command, string arguments, string workingFolder)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/c " + command + " " + arguments;
            p.StartInfo.WorkingDirectory = workingFolder;

            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;

            p.Start();

            var output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            var exitCode = p.ExitCode;

            return new ExternalCommandResult()
            {
                ExitCode = exitCode,
                RecordedStandardOutput = output
            };
        }
    }
}
