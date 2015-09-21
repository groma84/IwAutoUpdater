using IwAutoUpdater.CrossCutting.SFW.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.CrossCutting.SFW
{
    public class NowGetter : INowGetter
    {
        DateTime INowGetter.Now
        {
            get
            {
                return DateTime.Now;
            }
        }

        DateTime INowGetter.UtcNow
        {
            get
            {
                return DateTime.UtcNow;
            }
        }
    }
}
