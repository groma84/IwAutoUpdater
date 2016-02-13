using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;
using System.Linq;

namespace IwAutoUpdater.CrossCutting.Configuration.Test
{
    [TestClass]
    public class JsonFileConfigurationTest
    {
        ConfigurationFileAccessMock _configurationFileAccessMock = null;
        LoggerMock _loggerMock = null;
        IConfiguration _jsonFileConfiguration = null;

        string _path = "test-config.json";

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _configurationFileAccessMock = new ConfigurationFileAccessMock();
            _loggerMock = new LoggerMock();

            _jsonFileConfiguration = new JsonFileConfiguration(_configurationFileAccessMock, _loggerMock) as IConfiguration;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _loggerMock = null;
            _jsonFileConfiguration = null;
            _configurationFileAccessMock = null;
        }

        [TestMethod]
        public void JsonFileConfigurationTest_Get_BasicConfiguration()
        {
            var content = @"{
                          'global': {
                            'eMailSettings': {
                                'address': 'smtp.mailserver.com',
                              'username': 'mailUser',
                              'password': 'superSecure',
                                'useDefaultCredentials': 'false',
                            },
                            'eMailPickupDirectory': 'C:\\Mails\\Pickup\\',
                            'eMailSenderName': 'iwau@wow.com',
                            'workFolder': 'C:\\Temp\\IwAutoUpdater\\',
                            'checkIntervalMinutes': 15,
                            'resultMessageReceivers': [
                              {
                                'type': 'EMail',
                                'receiver': 'admin@mymail.com'
                              }
                            ]
                          },
                          'servers': [
                            {
                              'type': 'localFile',
                              'path': '//otherSystem/buildDir/Server1_Packaged.zip',                              
                              'downloadOnly': 'false',
                              'skipDatabaseUpdate': 'false',
                              'installerCommand': 'install-stuff.bat',
                              'installerCommandArguments':  'fromA toB',
                              'databaseUpdaterCommand': 'update-db.exe',
                              'databaseUpdaterCommandArguments': '--cs=""<<connectionString>>""',
                              'connectionString': 'Server=myServerAddress;Database=myDataBase;Integrated Security=true;',
                                'checkUrlsAfterInstallation': [
                                        'http://www.firsturl.de', 'https://localnetwork:7000/'
                                        ],
                                'checkUrlProxySettings': {
                                    'address': 'http://proxy:8080/',
                                    'username':  'user',
                                    'password':  'pw'
                                },
      'readVersionInfoFrom': '.\\Webmenue\\versioninfo.xml'
                            }
                          ]
                        }";
            _configurationFileAccessMock.ReadAllText.Add(_path, content);

            var actual = _jsonFileConfiguration.Get(_path);

            var expected = new Settings()
            {
                Global = new GlobalSettings()
                {
                    CheckIntervalMinutes = 15,
                    EMailSettings = new AddressUsernamePassword()
                    {
                        Password = "superSecure",
                        Address = "smtp.mailserver.com",
                        Username = "mailUser",
                        UseDefaultCredentials = false
                    },
                    EMailPickupDirectory = "C:\\Mails\\Pickup\\",
                    EMailSenderName = "iwau@wow.com",
                    ResultMessageReceivers = new MessageReceiver[]
                    {
                        new MessageReceiver()
                        {
                            Receiver = "admin@mymail.com",
                            Type = Base.MessageReceiverType.EMail
                        }
                    },
                    WorkFolder = "C:\\Temp\\IwAutoUpdater\\"
                },
                Servers = new ServerSettings[]
                {
                    new ServerSettings()
                    {
                        DownloadOnly = false,
                        Path = @"//otherSystem/buildDir/Server1_Packaged.zip",
                        SkipDatabaseUpdate = false,
                        Type = Base.GetDataMethod.LocalFile,
                        InstallerCommand = "install-stuff.bat",
                        InstallerCommandArguments = "fromA toB",
                        DatabaseUpdaterCommand = "update-db.exe",
                        DatabaseUpdaterCommandArguments = @"--cs=""<<connectionString>>""",
                        ConnectionString = "Server=myServerAddress;Database=myDataBase;Integrated Security=true;",
                        CheckUrlsAfterInstallation = new[] { "http://www.firsturl.de", "https://localnetwork:7000/" },
                        CheckUrlProxySettings = new AddressUsernamePassword()
                        {
                            Address = "http://proxy:8080/",
                            Password = "pw",
                            Username = "user"
                        },
                        ReadVersionInfoFrom = @".\Webmenue\versioninfo.xml"
                    }
                }
            };

            Assert.IsNotNull(actual);

            Assert.AreEqual(expected.Global.CheckIntervalMinutes, actual.Global.CheckIntervalMinutes);

            Assert.AreEqual(expected.Global.EMailSettings.Password, actual.Global.EMailSettings.Password);
            Assert.AreEqual(expected.Global.EMailSettings.Address, actual.Global.EMailSettings.Address);
            Assert.AreEqual(expected.Global.EMailSettings.Username, actual.Global.EMailSettings.Username);
            Assert.AreEqual(expected.Global.EMailSettings.UseDefaultCredentials, actual.Global.EMailSettings.UseDefaultCredentials);
            Assert.AreEqual(expected.Global.EMailPickupDirectory, actual.Global.EMailPickupDirectory);
            Assert.AreEqual(expected.Global.EMailSenderName, actual.Global.EMailSenderName);

            Assert.AreEqual(expected.Global.ResultMessageReceivers.ElementAt(0).Receiver, actual.Global.ResultMessageReceivers.ElementAt(0).Receiver);
            Assert.AreEqual(expected.Global.ResultMessageReceivers.ElementAt(0).Type, actual.Global.ResultMessageReceivers.ElementAt(0).Type);

            Assert.AreEqual(expected.Global.WorkFolder, actual.Global.WorkFolder);

            Assert.AreEqual(expected.Servers.ElementAt(0).Path, actual.Servers.ElementAt(0).Path);
            Assert.AreEqual(expected.Servers.ElementAt(0).Type, actual.Servers.ElementAt(0).Type);
            Assert.AreEqual(expected.Servers.ElementAt(0).DownloadOnly, actual.Servers.ElementAt(0).DownloadOnly);
            Assert.AreEqual(expected.Servers.ElementAt(0).SkipDatabaseUpdate, actual.Servers.ElementAt(0).SkipDatabaseUpdate);

            Assert.AreEqual(expected.Servers.ElementAt(0).InstallerCommand, actual.Servers.ElementAt(0).InstallerCommand);
            Assert.AreEqual(expected.Servers.ElementAt(0).InstallerCommandArguments, actual.Servers.ElementAt(0).InstallerCommandArguments);

            Assert.AreEqual(expected.Servers.ElementAt(0).DatabaseUpdaterCommand, actual.Servers.ElementAt(0).DatabaseUpdaterCommand);
            Assert.AreEqual(expected.Servers.ElementAt(0).DatabaseUpdaterCommandArguments, actual.Servers.ElementAt(0).DatabaseUpdaterCommandArguments);
            Assert.AreEqual(expected.Servers.ElementAt(0).ConnectionString, actual.Servers.ElementAt(0).ConnectionString);

            Assert.IsTrue(expected.Servers.ElementAt(0).CheckUrlsAfterInstallation.SequenceEqual(actual.Servers.ElementAt(0).CheckUrlsAfterInstallation));
            Assert.AreEqual(expected.Servers.ElementAt(0).CheckUrlProxySettings, actual.Servers.ElementAt(0).CheckUrlProxySettings);

            Assert.AreEqual(expected.Servers.ElementAt(0).ReadVersionInfoFrom, actual.Servers.ElementAt(0).ReadVersionInfoFrom);

        }
    }
}
