using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.CrossCutting.Base
{
    public abstract class Command
    {
        public abstract CommandResult Do(CommandResult resultOfPreviousCommand);
        public abstract string PackageName { get; }
        public abstract Command Copy();

        public Command RunAfterCompletedWithResultTrue;
        public Command RunAfterCompletedWithResultFalse;
    }
}
