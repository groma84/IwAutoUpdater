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

        public int DebugMessageCalled = 0;
        public Queue<string> DebugMessages = new Queue<string>();
        void ILogger.Debug(string message)
        {
            ++DebugMessageCalled;
            Debug.WriteLine(message);
            DebugMessages.Enqueue(message);
        }

        public int DebugComplexCalled = 0;
        void ILogger.Debug(string messageTemplate, params object[] data)
        {
            ++DebugComplexCalled;

            StringBuilder sb = new StringBuilder(messageTemplate.Replace("{", "<").Replace("}", ">") + Environment.NewLine);

            int cnt = -1;
            foreach (var d in data)
            {
                sb.Append("{").Append(++cnt).Append("}; ");
            }

            Debug.WriteLine(sb.ToString(), data);
        }

        void ILogger.Error(string message)
        {
            throw new NotImplementedException();
        }

        void ILogger.Error(string messageTemplate, params object[] data)
        {
            throw new NotImplementedException();
        }
    }
}