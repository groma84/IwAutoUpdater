using System.Collections.Generic;

namespace SFW.Contracts
{
    public interface IBlackboard
    {
        void Add(string key, BlackboardEntry entry);
        IEnumerable<BlackboardEntry> Get(string key);
        void Clear(string key);
    }
}
