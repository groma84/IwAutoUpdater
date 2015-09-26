namespace IwAutoUpdater.DAL.ExternalCommands.Contracts
{
    public interface IRunExternalCommand
    {
        ExternalCommandResult Run(string command, string arguments, string workingFolder);
    }
}
