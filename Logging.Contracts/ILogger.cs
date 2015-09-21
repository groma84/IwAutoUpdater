using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.CrossCutting.Logging.Contracts
{
    public interface ILogger
    {
        void Info(string message);
        void Info(string messageTemplate, params object[] data);
    }
}
