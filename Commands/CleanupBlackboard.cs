using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.DAL.Updates.Contracts;
using SFW.Contracts;

namespace IwAutoUpdater.BLL.Commands
{
    public class CleanupBlackboard : Command
    {
        private readonly IUpdatePackage _package;
        private readonly IBlackboard _blackboard;

        public CleanupBlackboard(IUpdatePackage package, IBlackboard blackboard)
        {
            _package = package;
            _blackboard = blackboard;
        }

        public override string PackageName
        {
            get
            {
                return _package.PackageName;
            }
        }

        public override CommandResult Do()
        {
            _blackboard.Clear(_package.PackageName);

            return new CommandResult(true);
        }

        public override Command Copy()
        {
            var x = new CleanupBlackboard(_package, _blackboard);
            x.RunAfterCompletedWithResultFalse = RunAfterCompletedWithResultFalse;
            x.RunAfterCompletedWithResultTrue = RunAfterCompletedWithResultTrue;
            x.AddResultsOfPreviousCommands(ResultsOfPreviousCommands);

            return x;
        }
    }
}
