using System;

namespace IwAutoUpdater.CrossCutting.SFW.Contracts
{
    public interface INowGetter
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }
}
