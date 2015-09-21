using IwAutoUpdater.CrossCutting.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.BLL.AutoUpdater.Test
{
    [TestClass]
    public class AutoUpdaterWorkerTest
    {
        AutoUpdaterWorker _autoUpdaterWorker;
        CommandMock _commandMock;
        CommandMock _commandMockCalledLaterOnTrue;
        CommandMock _commandMockCalledLaterOnFalse;
        ConcurrentDictionary<string, BlockingCollection<Command>> _queues;
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

            _queues = new ConcurrentDictionary<string, BlockingCollection<Command>>();

            _autoUpdaterWorker = new AutoUpdaterWorker(_loggerMock);
            _queues.TryAdd(_commandMock.PackageName, new BlockingCollection<Command>());

            _commandResult = new CommandResult();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _queues?.Clear();

            _commandMock = null;
            _commandMockCalledLaterOnTrue = null;
            _commandMockCalledLaterOnFalse = null;

            _loggerMock = null;

            _autoUpdaterWorker = null;

            CommandsProducerConsumer.Queues.Clear();
        }

        [TestMethod]
        public void AutoUpdaterWorkerTest_ExecuteOneCommand_CommandReturnsTrue_NoSecondCommandRegistered()
        {
            var myQueue = _queues.First();
            _autoUpdaterWorker.ExecuteOneCommand(_commandMock, _commandResult, myQueue, _queues);

            Assert.AreEqual(1, _commandMock.DoCalled);
            Assert.AreEqual(0, myQueue.Value.Count);
            Assert.IsTrue(myQueue.Value.IsCompleted);
        }


        [TestMethod]
        public void AutoUpdaterWorkerTest_ExecuteOneCommand_CommandReturnsTrue_TrueCommandQueued()
        {
            _commandMock.RunAfterCompletedWithResultTrue = _commandMockCalledLaterOnTrue;
            _commandMock.RunAfterCompletedWithResultFalse = _commandMockCalledLaterOnFalse;
            _commandMock.DoResult = new CommandResult(true);

            var myQueue = _queues.First();
            _autoUpdaterWorker.ExecuteOneCommand(_commandMock, _commandResult, myQueue, _queues);

            Assert.AreEqual(1, _commandMock.DoCalled);
            Assert.AreEqual(1, myQueue.Value.Count);
            Assert.AreEqual(_commandMockCalledLaterOnTrue, myQueue.Value.First());
            Assert.IsFalse(myQueue.Value.IsCompleted);
        }

        [TestMethod]
        public void AutoUpdaterWorkerTest_ExecuteOneCommand_CommandReturnsFalse_FalseCommandQueued()
        {
            _commandMock.RunAfterCompletedWithResultTrue = _commandMockCalledLaterOnTrue;
            _commandMock.RunAfterCompletedWithResultFalse = _commandMockCalledLaterOnFalse;
            _commandMock.DoResult = new CommandResult(false);

            var myQueue = _queues.First();
            _autoUpdaterWorker.ExecuteOneCommand(_commandMock, _commandResult, myQueue, _queues);

            Assert.AreEqual(1, _commandMock.DoCalled);
            Assert.AreEqual(1, myQueue.Value.Count);
            Assert.AreEqual(_commandMockCalledLaterOnFalse, myQueue.Value.First());
            Assert.IsFalse(myQueue.Value.IsCompleted);
        }

        [TestMethod]
        public void AutoUpdaterWorkerTest_OneWorkLoop_SmokeTest()
        {
            var secondCommandMock = new CommandMock() { PackageNameResult = "secondCommandMock" };

            CommandsProducerConsumer.Queues.TryAdd(_commandMock.PackageName, new BlockingCollection<Command>());
            CommandsProducerConsumer.Queues.TryAdd(secondCommandMock.PackageName, new BlockingCollection<Command>());

            var firstQueue = CommandsProducerConsumer.Queues[_commandMock.PackageName];
            var secondQueue = CommandsProducerConsumer.Queues[secondCommandMock.PackageName];
            firstQueue.Add(_commandMock);
            secondQueue.Add(secondCommandMock);

            _autoUpdaterWorker.OneWorkLoop();

            Assert.AreEqual(0, CommandsProducerConsumer.Queues.Count);
        }
    }
}
