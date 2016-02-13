using IwAutoUpdater.CrossCutting.SFW.Contracts;
using System;

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
