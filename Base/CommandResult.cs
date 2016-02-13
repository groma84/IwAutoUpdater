using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public override string ToString()
        {
            var errorTexts = new StringBuilder();

            foreach (var err in Errors.Select(a => a.Text + "-- " + a.Exception + "; "))
            {
                errorTexts.AppendLine(err);
            }
            
            return $"CommandResult: Successful: {Successful} - Errors: {errorTexts.ToString()}"; 
        }
    }
}
