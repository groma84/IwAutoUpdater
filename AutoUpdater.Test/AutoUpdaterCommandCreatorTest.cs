using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.DAL.Notifications.Contracts;
using IwAutoUpdater.DAL.Updates.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.BLL.AutoUpdater.Test
{
    [TestClass]
    public class AutoUpdaterCommandCreatorTest
    {
        AutoUpdaterCommandCreator _autoUpdaterCommandCreator;

        NowGetterMock _nowGetterMock;
        CheckTimerMock _checkTimerMock;
        ConfigurationConverterMock _configurationConverterMock;
        CommandBuilderMock _commandBuilderMock;
        Settings _settings;
        private LoggerMock _loggerMock;

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _settings = new Settings();
            _settings.Global = new GlobalSettings()
            {
                CheckIntervalMinutes = 5
            };

            _nowGetterMock = new NowGetterMock();
            _checkTimerMock = new CheckTimerMock();
            _configurationConverterMock = new ConfigurationConverterMock();
            _commandBuilderMock = new CommandBuilderMock();
            _loggerMock = new LoggerMock();

            _autoUpdaterCommandCreator = new AutoUpdaterCommandCreator(_checkTimerMock, _configurationConverterMock, _commandBuilderMock, _nowGetterMock, _loggerMock);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _autoUpdaterCommandCreator = null;

            _loggerMock = null;

        }

        [TestMethod]
        public void AutoUpdaterCommandCreatorTest_CalculateWaitTimeToNextMinute()
        {
            {
                _nowGetterMock.Now = new DateTime(2015, 5, 15, 11, 0, 53);
                var waitTime = _autoUpdaterCommandCreator.CalculateWaitTimeToNextMinute(_nowGetterMock);

                Assert.AreEqual(new TimeSpan(0, 0, 7), waitTime);
            }

            {
                _nowGetterMock.Now = new DateTime(2015, 5, 15, 11, 0, 0);
                var waitTime = _autoUpdaterCommandCreator.CalculateWaitTimeToNextMinute(_nowGetterMock);

                Assert.AreEqual(new TimeSpan(0, 0, 60), waitTime);
            }
        }

        [TestMethod]
        public void AutoUpdaterCommandCreatorTest_OneCheck_NoCheckNecessary()
        {
            // kein Check nötig, nichts darf aufgerufen werden
            _checkTimerMock.IsCheckForUpdatesNecessary = false;
            _autoUpdaterCommandCreator.CheckIfUpdateIsNecessaryAndEnqueueCommands(_settings, new IUpdatePackage[0], new INotificationReceiver[0]);
            Assert.AreEqual(1, _checkTimerMock.IsCheckForUpdatesNecessaryCalled);
        }

        [TestMethod]
        public void AutoUpdaterCommandCreatorTest_OneCheck_CheckNecessary()
        {
            //  Check nötig, Methoden müssen aufgerufen werden
            _checkTimerMock.IsCheckForUpdatesNecessary = true;
            _autoUpdaterCommandCreator.CheckIfUpdateIsNecessaryAndEnqueueCommands(_settings, new IUpdatePackage[0], new INotificationReceiver[0]);
            Assert.AreEqual(1, _checkTimerMock.IsCheckForUpdatesNecessaryCalled);
            Assert.AreEqual(1, _commandBuilderMock.GetCommandsCalled);
        }

        [TestMethod]
        public void AutoUpdaterCommandCreatorTest_OneCheck_CheckNecessary_CommandPackageAlreadyExists()
        {
            var mockPackage = new UpdatePackageMock()
            {
                PackageName = "UpdatePackageMock"
            };

            _commandBuilderMock.GetCommands.Add(new CommandMock()
            {
                PackageNameResult = mockPackage.PackageName
            });

            _checkTimerMock.IsCheckForUpdatesNecessary = true;
            _autoUpdaterCommandCreator.CheckIfUpdateIsNecessaryAndEnqueueCommands(_settings, new[] { mockPackage }, new INotificationReceiver[0]);
            Assert.AreEqual(1, _checkTimerMock.IsCheckForUpdatesNecessaryCalled);
            Assert.AreEqual(1, _commandBuilderMock.GetCommandsCalled);
            Assert.AreEqual(1, CommandsProducerConsumer.Queue.Count);

            //  Beim zweiten Mal darf das Command nicht nochmal eingereiht werden
            _autoUpdaterCommandCreator.CheckIfUpdateIsNecessaryAndEnqueueCommands(_settings, new[] { mockPackage }, new INotificationReceiver[0]);
            Assert.AreEqual(2, _checkTimerMock.IsCheckForUpdatesNecessaryCalled);
            Assert.AreEqual(2, _commandBuilderMock.GetCommandsCalled);
            Assert.AreEqual(1, CommandsProducerConsumer.Queue.Count);
        }
    }
}
