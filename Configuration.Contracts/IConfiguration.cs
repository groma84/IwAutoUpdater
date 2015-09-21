using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.CrossCutting.Configuration.Contracts
{
    public interface IConfiguration
    {
        Settings Get(string location);
    }
}
