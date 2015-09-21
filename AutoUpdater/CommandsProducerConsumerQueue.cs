using IwAutoUpdater.CrossCutting.Base;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.BLL.AutoUpdater
{
    public static class CommandsProducerConsumer
    {
        public static ConcurrentDictionary<string, BlockingCollection<Command>> Queues = new ConcurrentDictionary<string, BlockingCollection<Command>>();        
    }
}
