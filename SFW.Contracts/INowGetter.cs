using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.CrossCutting.SFW.Contracts
{
    public interface INowGetter
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }
}
