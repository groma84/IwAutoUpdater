using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.CrossCutting.Base
{
    public class CommandResult
    {
        public bool Successful;
        public IEnumerable<Error> Errors;

        public CommandResult()
        {
            Errors = new Error[0];
        }

        public CommandResult(bool successful)
                : this()
        {
            Successful = successful;
        }

        public CommandResult(bool successful, IEnumerable<Error> errorsInThisCommand)
            : this(successful)
        {
            Errors = errorsInThisCommand;
        }
    }
}
