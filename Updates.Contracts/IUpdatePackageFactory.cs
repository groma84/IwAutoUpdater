using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.DAL.Updates.Contracts
{
    public interface IUpdatePackageFactory
    {
        IUpdatePackage Create(ServerSettings serverSettings);
    }
}
