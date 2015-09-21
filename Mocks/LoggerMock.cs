using System;
using IwAutoUpdater.CrossCutting.Logging.Contracts;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;

namespace Mocks
{
    public class LoggerMock : ILogger
    {
        public int InfoMessageCalled = 0;
        public Queue<string> InfoMessages = new Queue<string>();
        void ILogger.Info(string message)
        {
            ++InfoMessageCalled;
            Debug.WriteLine(message);
            InfoMessages.Enqueue(message);
        }

        public int InfoComplexCalled = 0;
        void ILogger.Info(string messageTemplate, params object[] data)
        {
            ++InfoComplexCalled;

            StringBuilder sb = new StringBuilder(messageTemplate.Replace("{", "<").Replace("}", ">") + Environment.NewLine);

            int cnt = -1;
            foreach(var d in data)
            {
                sb.Append("{").Append(++cnt).Append("}; ");
            }

            Debug.WriteLine(sb.ToString(), data);
        }
    }
}