using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.CrossCutting.Base
{
    public abstract class Command
    {
        public abstract CommandResult Do();
        public abstract string PackageName { get; }
        public abstract Command Copy();

        public Command RunAfterCompletedWithResultTrue;
        public Command RunAfterCompletedWithResultFalse;

        private List<CommandResult> _resultsOfPreviousCommands = new List<CommandResult>();
        public IEnumerable<CommandResult> ResultsOfPreviousCommands { get { return _resultsOfPreviousCommands; } }        

        public void AddResultOfPreviousCommand(CommandResult resultOfPreviousCommand)
        {
            _resultsOfPreviousCommands.Add(resultOfPreviousCommand);
        }

        public void AddResultsOfPreviousCommands(IEnumerable<CommandResult> resultsOfPreviousCommands)
        {
            _resultsOfPreviousCommands.AddRange(resultsOfPreviousCommands);
        }
    }
}
