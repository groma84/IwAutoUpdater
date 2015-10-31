
// Datei generiert von BogusCallsForAssemblyCopy.tt
using System;
using System.Diagnostics;

namespace IwAutoUpdater.DIMappings
{
    public static class BogusCallsForAssemblyCopy
    {
        public static void DoBogusCalls()
        {
						var IAutoUpdaterCommandCreator = typeof(IwAutoUpdater.BLL.AutoUpdater.Contracts.IAutoUpdaterCommandCreator).ToString();
					Debug.WriteLine(IAutoUpdaterCommandCreator);

							var IAutoUpdaterThreadFactory = typeof(IwAutoUpdater.BLL.AutoUpdater.Contracts.IAutoUpdaterThreadFactory).ToString();
					Debug.WriteLine(IAutoUpdaterThreadFactory);

							var IAutoUpdaterWorker = typeof(IwAutoUpdater.BLL.AutoUpdater.Contracts.IAutoUpdaterWorker).ToString();
					Debug.WriteLine(IAutoUpdaterWorker);

							var AutoUpdaterCommandCreator = typeof(IwAutoUpdater.BLL.AutoUpdater.AutoUpdaterCommandCreator).ToString();
					Debug.WriteLine(AutoUpdaterCommandCreator);

							var AutoUpdaterThreadFactory = typeof(IwAutoUpdater.BLL.AutoUpdater.AutoUpdaterThreadFactory).ToString();
					Debug.WriteLine(AutoUpdaterThreadFactory);

							var AutoUpdaterWorker = typeof(IwAutoUpdater.BLL.AutoUpdater.AutoUpdaterWorker).ToString();
					Debug.WriteLine(AutoUpdaterWorker);

							var CommandsProducerConsumer = typeof(IwAutoUpdater.BLL.AutoUpdater.CommandsProducerConsumer).ToString();
					Debug.WriteLine(CommandsProducerConsumer);

							var ICheckTimer = typeof(IwAutoUpdater.BLL.CommandPlanner.Contracts.ICheckTimer).ToString();
					Debug.WriteLine(ICheckTimer);

							var ICommandBuilder = typeof(IwAutoUpdater.BLL.CommandPlanner.Contracts.ICommandBuilder).ToString();
					Debug.WriteLine(ICommandBuilder);

							var IConfigurationConverter = typeof(IwAutoUpdater.BLL.CommandPlanner.Contracts.IConfigurationConverter).ToString();
					Debug.WriteLine(IConfigurationConverter);

							var CheckTimer = typeof(IwAutoUpdater.BLL.CommandPlanner.CheckTimer).ToString();
					Debug.WriteLine(CheckTimer);

							var CommandBuilder = typeof(IwAutoUpdater.BLL.CommandPlanner.CommandBuilder).ToString();
					Debug.WriteLine(CommandBuilder);

							var ConfigurationConverter = typeof(IwAutoUpdater.BLL.CommandPlanner.ConfigurationConverter).ToString();
					Debug.WriteLine(ConfigurationConverter);

							var CheckIfNewer = typeof(IwAutoUpdater.BLL.Commands.CheckIfNewer).ToString();
					Debug.WriteLine(CheckIfNewer);

							var CheckUrlHttpStatusIs200 = typeof(IwAutoUpdater.BLL.Commands.CheckUrlHttpStatusIs200).ToString();
					Debug.WriteLine(CheckUrlHttpStatusIs200);

							var CleanupOldUnpackedFiles = typeof(IwAutoUpdater.BLL.Commands.CleanupOldUnpackedFiles).ToString();
					Debug.WriteLine(CleanupOldUnpackedFiles);

							var DeleteOldAndGetNewFile = typeof(IwAutoUpdater.BLL.Commands.DeleteOldAndGetNewFile).ToString();
					Debug.WriteLine(DeleteOldAndGetNewFile);

							var RunInstallerCommand = typeof(IwAutoUpdater.BLL.Commands.RunInstallerCommand).ToString();
					Debug.WriteLine(RunInstallerCommand);

							var SendErrorNotifications = typeof(IwAutoUpdater.BLL.Commands.SendErrorNotifications).ToString();
					Debug.WriteLine(SendErrorNotifications);

							var SendNotifications = typeof(IwAutoUpdater.BLL.Commands.SendNotifications).ToString();
					Debug.WriteLine(SendNotifications);

							var UnzipFile = typeof(IwAutoUpdater.BLL.Commands.UnzipFile).ToString();
					Debug.WriteLine(UnzipFile);

							var UpdateDatabase = typeof(IwAutoUpdater.BLL.Commands.UpdateDatabase).ToString();
					Debug.WriteLine(UpdateDatabase);

							var Command = typeof(IwAutoUpdater.CrossCutting.Base.Command).ToString();
					Debug.WriteLine(Command);

							var CommandResult = typeof(IwAutoUpdater.CrossCutting.Base.CommandResult).ToString();
					Debug.WriteLine(CommandResult);

							var DIAsTransientAttribute = typeof(IwAutoUpdater.CrossCutting.Base.DIAsTransientAttribute).ToString();
					Debug.WriteLine(DIAsTransientAttribute);

							var DIAsSingletonAttribute = typeof(IwAutoUpdater.CrossCutting.Base.DIAsSingletonAttribute).ToString();
					Debug.WriteLine(DIAsSingletonAttribute);

							var DIIgnoreAttribute = typeof(IwAutoUpdater.CrossCutting.Base.DIIgnoreAttribute).ToString();
					Debug.WriteLine(DIIgnoreAttribute);

							var Error = typeof(IwAutoUpdater.CrossCutting.Base.Error).ToString();
					Debug.WriteLine(Error);

							var ExtensionMethods = typeof(IwAutoUpdater.CrossCutting.Base.ExtensionMethods).ToString();
					Debug.WriteLine(ExtensionMethods);

							var GetDataMethod = typeof(IwAutoUpdater.CrossCutting.Base.GetDataMethod).ToString();
					Debug.WriteLine(GetDataMethod);

							var IHasVordraengelFaktor = typeof(IwAutoUpdater.CrossCutting.Base.IHasVordraengelFaktor).ToString();
					Debug.WriteLine(IHasVordraengelFaktor);

							var MessageReceiverType = typeof(IwAutoUpdater.CrossCutting.Base.MessageReceiverType).ToString();
					Debug.WriteLine(MessageReceiverType);

							var AddressUsernamePassword = typeof(IwAutoUpdater.CrossCutting.Configuration.Contracts.AddressUsernamePassword).ToString();
					Debug.WriteLine(AddressUsernamePassword);

							var GlobalSettings = typeof(IwAutoUpdater.CrossCutting.Configuration.Contracts.GlobalSettings).ToString();
					Debug.WriteLine(GlobalSettings);

							var IConfigurationFileAccess = typeof(IwAutoUpdater.CrossCutting.Configuration.Contracts.IConfigurationFileAccess).ToString();
					Debug.WriteLine(IConfigurationFileAccess);

							var MessageReceiver = typeof(IwAutoUpdater.CrossCutting.Configuration.Contracts.MessageReceiver).ToString();
					Debug.WriteLine(MessageReceiver);

							var ServerSettings = typeof(IwAutoUpdater.CrossCutting.Configuration.Contracts.ServerSettings).ToString();
					Debug.WriteLine(ServerSettings);

							var Settings = typeof(IwAutoUpdater.CrossCutting.Configuration.Contracts.Settings).ToString();
					Debug.WriteLine(Settings);

							var IConfiguration = typeof(IwAutoUpdater.CrossCutting.Configuration.Contracts.IConfiguration).ToString();
					Debug.WriteLine(IConfiguration);

							var ConfigurationFileAccess = typeof(IwAutoUpdater.CrossCutting.Configuration.ConfigurationFileAccess).ToString();
					Debug.WriteLine(ConfigurationFileAccess);

							var JsonFileConfiguration = typeof(IwAutoUpdater.CrossCutting.Configuration.JsonFileConfiguration).ToString();
					Debug.WriteLine(JsonFileConfiguration);

							var ILogger = typeof(IwAutoUpdater.CrossCutting.Logging.Contracts.ILogger).ToString();
					Debug.WriteLine(ILogger);

							var Logger = typeof(IwAutoUpdater.CrossCutting.Logging.Logger).ToString();
					Debug.WriteLine(Logger);

							var INowGetter = typeof(IwAutoUpdater.CrossCutting.SFW.Contracts.INowGetter).ToString();
					Debug.WriteLine(INowGetter);

							var NowGetter = typeof(IwAutoUpdater.CrossCutting.SFW.NowGetter).ToString();
					Debug.WriteLine(NowGetter);

							var ISendMail = typeof(IwAutoUpdater.DAL.EMails.Contracts.ISendMail).ToString();
					Debug.WriteLine(ISendMail);

							var MailData = typeof(IwAutoUpdater.DAL.EMails.Contracts.MailData).ToString();
					Debug.WriteLine(MailData);

							var SendMail = typeof(IwAutoUpdater.DAL.EMails.SendMail).ToString();
					Debug.WriteLine(SendMail);

							var ExternalCommandResult = typeof(IwAutoUpdater.DAL.ExternalCommands.Contracts.ExternalCommandResult).ToString();
					Debug.WriteLine(ExternalCommandResult);

							var IRunExternalCommand = typeof(IwAutoUpdater.DAL.ExternalCommands.Contracts.IRunExternalCommand).ToString();
					Debug.WriteLine(IRunExternalCommand);

							var ExternalCommandRunner = typeof(IwAutoUpdater.DAL.ExternalCommands.ExternalCommandRunner).ToString();
					Debug.WriteLine(ExternalCommandRunner);

							var IDirectory = typeof(IwAutoUpdater.DAL.LocalFiles.Contracts.IDirectory).ToString();
					Debug.WriteLine(IDirectory);

							var ISingleFile = typeof(IwAutoUpdater.DAL.LocalFiles.Contracts.ISingleFile).ToString();
					Debug.WriteLine(ISingleFile);

							var Directory = typeof(IwAutoUpdater.DAL.LocalFiles.Directory).ToString();
					Debug.WriteLine(Directory);

							var SingleFile = typeof(IwAutoUpdater.DAL.LocalFiles.SingleFile).ToString();
					Debug.WriteLine(SingleFile);

							var INotificationReceiver = typeof(IwAutoUpdater.DAL.Notifications.Contracts.INotificationReceiver).ToString();
					Debug.WriteLine(INotificationReceiver);

							var INotificationReceiverFactory = typeof(IwAutoUpdater.DAL.Notifications.Contracts.INotificationReceiverFactory).ToString();
					Debug.WriteLine(INotificationReceiverFactory);

							var NotificationSentException = typeof(IwAutoUpdater.DAL.Notifications.Contracts.NotificationSentException).ToString();
					Debug.WriteLine(NotificationSentException);

							var MailReceiver = typeof(IwAutoUpdater.DAL.Notifications.MailReceiver).ToString();
					Debug.WriteLine(MailReceiver);

							var NotificationReceiverFactory = typeof(IwAutoUpdater.DAL.Notifications.NotificationReceiverFactory).ToString();
					Debug.WriteLine(NotificationReceiverFactory);

							var IUpdatePackage = typeof(IwAutoUpdater.DAL.Updates.Contracts.IUpdatePackage).ToString();
					Debug.WriteLine(IUpdatePackage);

							var IUpdatePackageAccess = typeof(IwAutoUpdater.DAL.Updates.Contracts.IUpdatePackageAccess).ToString();
					Debug.WriteLine(IUpdatePackageAccess);

							var IUpdatePackageAccessFactory = typeof(IwAutoUpdater.DAL.Updates.Contracts.IUpdatePackageAccessFactory).ToString();
					Debug.WriteLine(IUpdatePackageAccessFactory);

							var IUpdatePackageFactory = typeof(IwAutoUpdater.DAL.Updates.Contracts.IUpdatePackageFactory).ToString();
					Debug.WriteLine(IUpdatePackageFactory);

							var DefaultUpdatePackage = typeof(IwAutoUpdater.DAL.Updates.DefaultUpdatePackage).ToString();
					Debug.WriteLine(DefaultUpdatePackage);

							var LocalFileAccess = typeof(IwAutoUpdater.DAL.Updates.LocalFileAccess).ToString();
					Debug.WriteLine(LocalFileAccess);

							var SmbFileAccess = typeof(IwAutoUpdater.DAL.Updates.SmbFileAccess).ToString();
					Debug.WriteLine(SmbFileAccess);

							var UpdatePackageAccessFactory = typeof(IwAutoUpdater.DAL.Updates.UpdatePackageAccessFactory).ToString();
					Debug.WriteLine(UpdatePackageAccessFactory);

							var UpdatePackageFactory = typeof(IwAutoUpdater.DAL.Updates.UpdatePackageFactory).ToString();
					Debug.WriteLine(UpdatePackageFactory);

							var HtmlDownload = typeof(IwAutoUpdater.DAL.WebAccess.Contracts.HtmlDownload).ToString();
					Debug.WriteLine(HtmlDownload);

							var IHtmlGetter = typeof(IwAutoUpdater.DAL.WebAccess.Contracts.IHtmlGetter).ToString();
					Debug.WriteLine(IHtmlGetter);

							var ProxySettings = typeof(IwAutoUpdater.DAL.WebAccess.Contracts.ProxySettings).ToString();
					Debug.WriteLine(ProxySettings);

							var HtmlGetter = typeof(IwAutoUpdater.DAL.WebAccess.HtmlGetter).ToString();
					Debug.WriteLine(HtmlGetter);

							var WebClientWithTimeout = typeof(IwAutoUpdater.DAL.WebAccess.WebClientWithTimeout).ToString();
					Debug.WriteLine(WebClientWithTimeout);

							var BogusCallsForAssemblyCopy = typeof(IwAutoUpdater.DIMappings.BogusCallsForAssemblyCopy).ToString();
					Debug.WriteLine(BogusCallsForAssemblyCopy);

							var Container = typeof(IwAutoUpdater.DIMappings.Container).ToString();
					Debug.WriteLine(Container);

			        }
    }
}