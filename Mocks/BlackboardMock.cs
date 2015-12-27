using System;
using System.Collections.Generic;
using SFW.Contracts;

namespace Mocks
{
    public class BlackboardMock : IBlackboard
    {
        void IBlackboard.Add(string key, BlackboardEntry entry)
        {
            throw new NotImplementedException();
        }

        IEnumerable<BlackboardEntry> IBlackboard.Get(string key)
        {
            throw new NotImplementedException();
        }
    }
}