using IwAutoUpdater.BLL.CommandPlanner.Contracts;
using IwAutoUpdater.BLL.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;
using System.IO;
using System.Linq;
using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using SFW.Contracts;
using Moq;
using IwAutoUpdater.DAL.WebAccess.Contracts;

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
        Mock<IHtmlGetter> _htmlGetterMock;
        NowGetterMock _nowGetterMock;
        Mock<IBlackboard> _blackboardMock;

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
                DatabaseUpdaterCommand = "connectionString",
                CheckUrlsAfterInstallation = new[] { "fakeCheckUrl1", "fakeCheckUrl2" },
            };

            _singleFileMock = new SingleFileMock();
            _loggerMock = new LoggerMock();
            _directoryMock = new DirectoryMock();
            _runExternalCommandMock = new RunExternalCommandMock();
            _htmlGetterMock = new Mock<IHtmlGetter>();
            _nowGetterMock = new NowGetterMock();
            _blackboardMock = new Mock<IBlackboard>();

            _commandBuilder = new CommandBuilder(_singleFileMock, _directoryMock, _loggerMock, _runExternalCommandMock, _htmlGetterMock.Object, _nowGetterMock, _blackboardMock.Object);
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
            _updatePackageMock.Settings.ReadVersionInfoFrom = _fileName;

            var actual = _commandBuilder.GetCommands(_workFolder, new[] { _updatePackageMock }, new[] { _mockMailReceiver }).ToArray();
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(typeof(CheckIfNewer), actual[0].GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(DeleteOldAndGetNewFile), actual[0].RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(CleanupOldUnpackedFiles), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(UnzipFile), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(RunInstallerCommand), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(UpdateDatabase), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            // zwei zu prüfende Urls -> zweimal das Command drin
            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(CheckUrlHttpStatusIs200), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());
            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(CheckUrlHttpStatusIs200), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(GetVersionInfo), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(SendNotifications), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(CleanupBlackboard), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            // "immer wenn wir die Default-Queue erweitern, muss der Test angepasst werden"
            Assert.IsNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
        }

        [TestMethod]
        public void CommandBuilderTest_GetCommands_OneServerAndOneReceiver_DownloadOnly()
        {
            _updatePackageMock.Settings.DownloadOnly = true;
            _updatePackageMock.Settings.ReadVersionInfoFrom = _fileName;

            var actual = _commandBuilder.GetCommands(_workFolder, new[] { _updatePackageMock }, new[] { _mockMailReceiver }).ToArray();
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(typeof(CheckIfNewer), actual[0].GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(DeleteOldAndGetNewFile), actual[0].RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(CleanupOldUnpackedFiles), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(UnzipFile), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(GetVersionInfo), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(SendNotifications), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(CleanupBlackboard), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            // "immer wenn wir die Default-Queue ändern, muss der Test angepasst werden"
            Assert.IsNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
        }

        [TestMethod]
        public void CommandBuilderTest_GetCommands_OneServerAndOneReceiver_SkipDatabaseUpdate()
        {
            _updatePackageMock.Settings.SkipDatabaseUpdate = true;
            _updatePackageMock.Settings.ReadVersionInfoFrom = _fileName;

            var actual = _commandBuilder.GetCommands(_workFolder, new[] { _updatePackageMock }, new[] { _mockMailReceiver }).ToArray();
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(typeof(CheckIfNewer), actual[0].GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(DeleteOldAndGetNewFile), actual[0].RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(CleanupOldUnpackedFiles), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(UnzipFile), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(RunInstallerCommand), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            // zwei zu prüfende Urls -> zweimal das Command drin
            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(CheckUrlHttpStatusIs200), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());
            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(CheckUrlHttpStatusIs200), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(GetVersionInfo), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(SendNotifications), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(CleanupBlackboard), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());
            
            // "immer wenn wir die Default-Queue ändern, muss der Test angepasst werden"
            Assert.IsNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
        }

        [TestMethod]
        public void CommandBuilderTest_GetCommands_OneServerAndOneReceiver_KeineZuPruefendenUrls()
        {
            _updatePackageMock.Settings.CheckUrlsAfterInstallation = new string[0];
            _updatePackageMock.Settings.ReadVersionInfoFrom = _fileName;

            var actual = _commandBuilder.GetCommands(_workFolder, new[] { _updatePackageMock }, new[] { _mockMailReceiver }).ToArray();
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(typeof(CheckIfNewer), actual[0].GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(DeleteOldAndGetNewFile), actual[0].RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(CleanupOldUnpackedFiles), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(UnzipFile), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(RunInstallerCommand), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(UpdateDatabase), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(GetVersionInfo), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(SendNotifications), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            Assert.IsNotNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
            Assert.AreEqual(typeof(CleanupBlackboard), actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.GetType());

            // "immer wenn wir die Default-Queue erweitern, muss der Test angepasst werden"
            Assert.IsNull(actual[0].RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue.RunAfterCompletedWithResultTrue);
        }
    }
}
