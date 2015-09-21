using System;
using IwAutoUpdater.CrossCutting.SFW.Contracts;

namespace Mocks
{
    public class NowGetterMock : INowGetter
    {
        public DateTime Now = DateTime.Now;
        DateTime INowGetter.Now
        {
            get
            {
                return Now;
            }
        }

        public DateTime UtcNow = DateTime.UtcNow;
        DateTime INowGetter.UtcNow
        {
            get
            {
                return UtcNow;
            }
        }
    }
}