using IwAutoUpdater.CrossCutting.Logging.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IwAutoUpdater.CrossCutting.Logging.Test
{
    [TestClass]
    public class LoggerTest
    {
        private ILogger _logger;

        string _message = "TestMessage";

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _logger = new Logger();
        }

        [TestCleanup]
        public void TestCleanup()
        {
           
        }

        [TestMethod]
        public void LoggerTest_LogInfo()
        {
            _logger.Info(_message);
        }
    }
}
