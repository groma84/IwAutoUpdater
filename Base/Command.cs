using System.Collections.Generic;

namespace IwAutoUpdater.CrossCutting.Base
{
    public abstract class Command : IHasVordraengelFaktor
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

        protected int? _vordraengelFaktor;
        int IHasVordraengelFaktor.GetVordraengelFaktor()
        {
            if (!_vordraengelFaktor.HasValue)
            {
                // so sorgen wir dafür, dass jedes Command eines Pakets einen nachvollziehbaren vordraengelFaktor hat
                // und außerdem alle Commands eines Pakets gemeinsam bearbeitet werden, sofern sie nicht explizit über einen vorab
                // gesetzten vordraengelFaktor anders "einsortiert" wurden
                _vordraengelFaktor = PackageName.GetHashCode();
            }
            return _vordraengelFaktor.Value;
        }
    }
}
