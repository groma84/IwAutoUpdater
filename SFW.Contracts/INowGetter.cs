using System;

namespace IWAutoUpdater.CrossCutting.SFW.Contracts
{
    public interface INowGetter
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }
}
