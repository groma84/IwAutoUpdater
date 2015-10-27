using IwAutoUpdater.CrossCutting.Base;
using System.Collections.Concurrent;

namespace IwAutoUpdater.BLL.AutoUpdater
{
    public static class CommandsProducerConsumer
    {
        public static BlockingCollection<Command> Queue = new BlockingCollection<Command>(new ProducerConsumerCollectionWithVordraengelFaktor<Command>());        
    }
}
