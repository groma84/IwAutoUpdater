using IwAutoUpdater.CrossCutting.Base;
using SFW.Contracts;
using System.Collections.Generic;

namespace SFW
{
    [DIAsSingleton]
    public class Blackboard : IBlackboard
    {
        Dictionary<string, List<BlackboardEntry>> _blackboard;

        public Blackboard()
        {
            _blackboard = new Dictionary<string, List<BlackboardEntry>>();
        }

        void IBlackboard.Add(string key, BlackboardEntry entry)
        {
            CreateKeyIfNotExists(key);
            _blackboard[key].Add(entry);
        }

        IEnumerable<BlackboardEntry> IBlackboard.Get(string key)
        {
            CreateKeyIfNotExists(key);
            return _blackboard[key];
        }

        void CreateKeyIfNotExists(string key)
        {
            if (!_blackboard.ContainsKey(key))
            {
                _blackboard[key] = new List<BlackboardEntry>();
            }
        }
    }
}
