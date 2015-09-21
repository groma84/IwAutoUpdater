using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                                        'Server': 'smtp.mailserver.com',
                              'Username': 'mailUser',
                              'Password': 'superSecure'
                            },
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
                              'databaseUpdateConnectionString': 'Server = x; Initial Catalog = y;',
                                'databaseScriptSubfolder': '/DBUpdater/DDLs/'
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
                    EMailSettings = new EMailSettings()
                    {
                        Password = "superSecure",
                        Server = "smtp.mailserver.com",
                        Username = "mailUser"
                    },
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
                        DatabaseUpdateConnectionString = "Server = x; Initial Catalog = y;",
                        DatabaseScriptSubfolder = @"/DBUpdater/DDLs/",
                    }
                }
            };

            Assert.IsNotNull(actual);

            Assert.AreEqual(expected.Global.CheckIntervalMinutes, actual.Global.CheckIntervalMinutes);
            Assert.AreEqual(expected.Global.EMailSettings.Password, actual.Global.EMailSettings.Password);
            Assert.AreEqual(expected.Global.EMailSettings.Server, actual.Global.EMailSettings.Server);
            Assert.AreEqual(expected.Global.EMailSettings.Username, actual.Global.EMailSettings.Username);
            Assert.AreEqual(expected.Global.ResultMessageReceivers.ElementAt(0).Receiver, actual.Global.ResultMessageReceivers.ElementAt(0).Receiver);
            Assert.AreEqual(expected.Global.ResultMessageReceivers.ElementAt(0).Type, actual.Global.ResultMessageReceivers.ElementAt(0).Type);
            Assert.AreEqual(expected.Global.WorkFolder, actual.Global.WorkFolder);

            Assert.AreEqual(expected.Servers.ElementAt(0).Path, actual.Servers.ElementAt(0).Path);
            Assert.AreEqual(expected.Servers.ElementAt(0).Type, actual.Servers.ElementAt(0).Type);
            Assert.AreEqual(expected.Servers.ElementAt(0).DownloadOnly, actual.Servers.ElementAt(0).DownloadOnly);
            Assert.AreEqual(expected.Servers.ElementAt(0).SkipDatabaseUpdate, actual.Servers.ElementAt(0).SkipDatabaseUpdate);
            Assert.AreEqual(expected.Servers.ElementAt(0).InstallerCommand, actual.Servers.ElementAt(0).InstallerCommand);
            Assert.AreEqual(expected.Servers.ElementAt(0).InstallerCommandArguments, actual.Servers.ElementAt(0).InstallerCommandArguments);
            Assert.AreEqual(expected.Servers.ElementAt(0).DatabaseUpdateConnectionString, actual.Servers.ElementAt(0).DatabaseUpdateConnectionString);
            Assert.AreEqual(expected.Servers.ElementAt(0).DatabaseScriptSubfolder, actual.Servers.ElementAt(0).DatabaseScriptSubfolder);
        }
    }
}
