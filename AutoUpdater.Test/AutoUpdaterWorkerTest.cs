using IwAutoUpdater.CrossCutting.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;
using System.Collections.Concurrent;
using System.Linq;

namespace IwAutoUpdater.BLL.AutoUpdater.Test
{
    [TestClass]
    public class AutoUpdaterWorkerTest
    {
        AutoUpdaterWorker _autoUpdaterWorker;
        CommandMock _commandMock;
        CommandMock _commandMockCalledLaterOnTrue;
        CommandMock _commandMockCalledLaterOnFalse;
        BlockingCollection<Command> _queue;
        LoggerMock _loggerMock;
        CommandResult _commandResult;

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _commandMock = new CommandMock() { PackageNameResult = "AutoUpdaterWorkerTest" };
            _commandMockCalledLaterOnTrue = new CommandMock() { PackageNameResult = "AutoUpdaterWorkerTest" };
            _commandMockCalledLaterOnFalse = new CommandMock() { PackageNameResult = "AutoUpdaterWorkerTest" };

            _loggerMock = new LoggerMock();

            _queue = new BlockingCollection<Command>();

            _autoUpdaterWorker = new AutoUpdaterWorker(_loggerMock);
           
            _commandResult = new CommandResult();
        }

        [TestCleanup]
        public void TestCleanup()
        {           
            _commandMock = null;
            _commandMockCalledLaterOnTrue = null;
            _commandMockCalledLaterOnFalse = null;

            _loggerMock = null;

            _autoUpdaterWorker = null;

            _queue = null;

            CommandsProducerConsumer.Queue = null;
        }

        [TestMethod]
        public void AutoUpdaterWorkerTest_ExecuteOneCommand_CommandReturnsTrue_NoSecondCommandRegistered()
        {
            _autoUpdaterWorker.ExecuteOneCommand(_commandMock, _queue);

            Assert.AreEqual(1, _commandMock.DoCalled);
            Assert.AreEqual(0, _queue.Count);
        }


        [TestMethod]
        public void AutoUpdaterWorkerTest_ExecuteOneCommand_CommandReturnsTrue_TrueCommandQueued()
        {
            _commandMock.RunAfterCompletedWithResultTrue = _commandMockCalledLaterOnTrue;
            _commandMock.RunAfterCompletedWithResultFalse = _commandMockCalledLaterOnFalse;
            _commandMock.DoResult = new CommandResult(true);

            _autoUpdaterWorker.ExecuteOneCommand(_commandMock, _queue);

            Assert.AreEqual(1, _commandMock.DoCalled);
            Assert.AreEqual(1, _queue.Count);
            Assert.AreEqual(_commandMockCalledLaterOnTrue, _queue.First());
            Assert.IsFalse(_queue.IsCompleted);
        }

        [TestMethod]
        public void AutoUpdaterWorkerTest_ExecuteOneCommand_CommandReturnsFalse_FalseCommandQueued()
        {
            _commandMock.RunAfterCompletedWithResultTrue = _commandMockCalledLaterOnTrue;
            _commandMock.RunAfterCompletedWithResultFalse = _commandMockCalledLaterOnFalse;
            _commandMock.DoResult = new CommandResult(false);

            _autoUpdaterWorker.ExecuteOneCommand(_commandMock, _queue);

            Assert.AreEqual(1, _commandMock.DoCalled);
            Assert.AreEqual(1, _queue.Count);
            Assert.AreEqual(_commandMockCalledLaterOnFalse, _queue.First());
            Assert.IsFalse(_queue.IsCompleted);
        }       
    }
}
