using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.DAL.LocalFiles.Contracts
{
    public interface IDatabaseScript
    {
        DatabaseScript Load(string filePath);
    }
}
