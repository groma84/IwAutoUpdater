using IwAutoUpdater.BLL.CommandPlanner.Contracts;
using IwAutoUpdater.BLL.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IwAutoUpdater.DAL.LocalFiles.Contracts;
using IwAutoUpdater.CrossCutting.Configuration.Contracts;

namespace IwAutoUpdater.BLL.CommandPlanner.Test
{
    [TestClass]
    public class CommandBuilderTest
    {
        ICommandBuilder _commandBuilder;
        SingleFileMock _singleFileMock;
        UpdatePackageMock _updatePackageMock;
        UpdatePackageAccessMock _updatePackageAccessMock;
        NotificationReceiverMock _mockMailReceiver = new NotificationReceiverMock();
        LoggerMock _loggerMock;
        DirectoryMock _directoryMock;
        RunExternalCommandMock _runExternalCommandMock;

        static string _workFolder = @"C:\zork\";
        static string _fileName = "zork.zip";
        string _fullPath = Path.Combine(_workFolder, _fileName);

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _updatePackageAccessMock = new UpdatePackageAccessMock();
            _updatePackageAccessMock.GetFilenameOnly = _fileName; // wird sowieso nie echt drauf zugegriffen
            _updatePackageMock = new UpdatePackageMock() { Access = _updatePackageAccessMock };
            _updatePackageMock.Settings = new ServerSettings()
            {
                DatabaseUpdaterCommandArguments = "ddl",
                DatabaseUpdaterCommand = "connectionString"
            };

            _singleFileMock = new SingleFileMock();
            _loggerMock = new LoggerMock();
            _directoryMock = new DirectoryMock();
            _runExternalCommandMock = new RunExternalCommandMock();

            _commandBuilder = new CommandBuilder(_singleFileMock, _directoryMock, _loggerMock, _runExternalCommandMock);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _commandBuilder = null;

            _updatePackageMock = null;
            _singleFileMock = null;
            _loggerMock = null;
            _directoryMock = null;
        }

        [TestMethod]
        public void CommandBuilderTest_GetCommands_OneServerAndOneReceiver_FullCommandQueue()
        {
            var actual = _commandBuilder.GetCommands(_workFolder, new[] { _updatePackageMock }, new[] { _mockMailReceiver }).ToArray();
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(typeof(CheckIfNewer), actual[0].GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(GetFile), actual[0].RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(CleanupOldUnpackedFiles), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(UnzipFile), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(RunInstallerCommand), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(UpdateDatabase), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            // "immer wenn wir die Default-Queue erweitern, muss der Test angepasst werden"
            Assert.IsNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
        }

        [TestMethod]
        public void CommandBuilderTest_GetCommands_OneServerAndOneReceiver_DownloadOnly()
        {
            _updatePackageMock.Settings.DownloadOnly = true;

            var actual = _commandBuilder.GetCommands(_workFolder, new[] { _updatePackageMock }, new[] { _mockMailReceiver }).ToArray();
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(typeof(CheckIfNewer), actual[0].GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(GetFile), actual[0].RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(CleanupOldUnpackedFiles), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(UnzipFile), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            // "immer wenn wir die Default-Queue erweitern, muss der Test angepasst werden"
            Assert.IsNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
        }

        [TestMethod]
        public void CommandBuilderTest_GetCommands_OneServerAndOneReceiver_SkipDatabaseUpdate()
        {
            _updatePackageMock.Settings.SkipDatabaseUpdate = true;

            var actual = _commandBuilder.GetCommands(_workFolder, new[] { _updatePackageMock }, new[] { _mockMailReceiver }).ToArray();
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(typeof(CheckIfNewer), actual[0].GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(GetFile), actual[0].RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(CleanupOldUnpackedFiles), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(UnzipFile), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(RunInstallerCommand), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            // "immer wenn wir die Default-Queue erweitern, muss der Test angepasst werden"
            Assert.IsNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
        }
    }
}
