using IwAutoUpdater.DAL.ExternalCommands.Contracts;

namespace Mocks
{
    public class RunExternalCommandMock : IRunExternalCommand
    {
        public ExternalCommandResult Run = null;
        public int RunCalled = 0;
        ExternalCommandResult IRunExternalCommand.Run(string command, string arguments, string workingFolder)
        {
            ++RunCalled;
            return Run;
        }
    }
}