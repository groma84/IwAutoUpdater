using System.Collections.Generic;
using SFW.Contracts;

namespace Mocks
{
    public class BlackboardMock : IBlackboard
    {
        public int AddCalls = 0;
        public Dictionary<string, List<BlackboardEntry>> Added = new Dictionary<string, List<BlackboardEntry>>();
        void IBlackboard.Add(string key, BlackboardEntry entry)
        {
            ++AddCalls;
            if(!Added.ContainsKey(key))
            {
                Added[key] = new List<BlackboardEntry>();
            }
            Added[key].Add(entry);
        }

        public int GetCalls = 0;
        public List<BlackboardEntry> Get = new List<BlackboardEntry>();
        IEnumerable<BlackboardEntry> IBlackboard.Get(string key)
        {
            ++GetCalls;
            return Get;
        }
    }
}