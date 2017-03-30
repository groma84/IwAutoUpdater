
// Datei generiert von BogusCallsForAssemblyCopy.tt
using System.Diagnostics;

namespace IwAutoUpdater.DIMappings
{
    // MSBuild schmeisst DLLs raus, die nirgends im Code aufgerufen werden, auch wenn sie auf CopyLocal stehen und
    // explizit als Projekt-Referenz eingetragen sind. Daher faken wir hier Aufrufe (die nie ausgeführt werden) *seufz*.
    public static class BogusCallsForAssemblyCopy
    {
        public static void DoBogusCalls()
        {
					// IwAutoUpdater.BLL.AutoUpdater.Contracts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
				var IAutoUpdaterCommandCreator = typeof(IwAutoUpdater.BLL.AutoUpdater.Contracts.IAutoUpdaterCommandCreator).ToString();
					Debug.WriteLine(IAutoUpdaterCommandCreator);

						// IwAutoUpdater.BLL.CommandPlanner.Contracts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
				var ICheckTimer = typeof(IwAutoUpdater.BLL.CommandPlanner.Contracts.ICheckTimer).ToString();
					Debug.WriteLine(ICheckTimer);

						// IwAutoUpdater.CrossCutting.Configuration.Contracts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
				var AddressUsernamePassword = typeof(IwAutoUpdater.CrossCutting.Configuration.Contracts.AddressUsernamePassword).ToString();
					Debug.WriteLine(AddressUsernamePassword);

						// IwAutoUpdater.CrossCutting.Logging.Contracts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
				var ILogger = typeof(IwAutoUpdater.CrossCutting.Logging.Contracts.ILogger).ToString();
					Debug.WriteLine(ILogger);

						// IwAutoUpdater.CrossCutting.SFW.Contracts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
				var INowGetter = typeof(IwAutoUpdater.CrossCutting.SFW.Contracts.INowGetter).ToString();
					Debug.WriteLine(INowGetter);

						// IwAutoUpdater.DAL.EMails.Contracts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
				var ISendMail = typeof(IwAutoUpdater.DAL.EMails.Contracts.ISendMail).ToString();
					Debug.WriteLine(ISendMail);

						// IwAutoUpdater.DAL.ExternalCommands.Contracts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
				var ExternalCommandResult = typeof(IwAutoUpdater.DAL.ExternalCommands.Contracts.ExternalCommandResult).ToString();
					Debug.WriteLine(ExternalCommandResult);

						// IwAutoUpdater.DAL.LocalFiles.Contracts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
				var IDirectory = typeof(IwAutoUpdater.DAL.LocalFiles.Contracts.IDirectory).ToString();
					Debug.WriteLine(IDirectory);

						// IwAutoUpdater.DAL.Notifications.Contracts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
				var INotificationReceiver = typeof(IwAutoUpdater.DAL.Notifications.Contracts.INotificationReceiver).ToString();
					Debug.WriteLine(INotificationReceiver);

						// IwAutoUpdater.DAL.Updates.Contracts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
				var IUpdatePackage = typeof(IwAutoUpdater.DAL.Updates.Contracts.IUpdatePackage).ToString();
					Debug.WriteLine(IUpdatePackage);

						// IwAutoUpdater.DAL.WebAccess.Contracts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
				var HtmlDownload = typeof(IwAutoUpdater.DAL.WebAccess.Contracts.HtmlDownload).ToString();
					Debug.WriteLine(HtmlDownload);

									// IwAutoUpdater.BLL.AutoUpdater, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
							var AutoUpdaterCommandCreator = typeof(IwAutoUpdater.BLL.AutoUpdater.AutoUpdaterCommandCreator).ToString();
								Debug.WriteLine(AutoUpdaterCommandCreator);
												// IwAutoUpdater.BLL.CommandPlanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
							var CheckTimer = typeof(IwAutoUpdater.BLL.CommandPlanner.CheckTimer).ToString();
								Debug.WriteLine(CheckTimer);
												// IwAutoUpdater.CrossCutting.Configuration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
							var ConfigurationFileAccess = typeof(IwAutoUpdater.CrossCutting.Configuration.ConfigurationFileAccess).ToString();
								Debug.WriteLine(ConfigurationFileAccess);
												// IwAutoUpdater.CrossCutting.Logging, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
							var Logger = typeof(IwAutoUpdater.CrossCutting.Logging.Logger).ToString();
								Debug.WriteLine(Logger);
												// IwAutoUpdater.CrossCutting.SFW, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
							var NowGetter = typeof(IwAutoUpdater.CrossCutting.SFW.NowGetter).ToString();
								Debug.WriteLine(NowGetter);
												// IwAutoUpdater.DAL.EMails, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
							var SendMail = typeof(IwAutoUpdater.DAL.EMails.SendMail).ToString();
								Debug.WriteLine(SendMail);
												// IwAutoUpdater.DAL.ExternalCommands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
							var ExternalCommandRunner = typeof(IwAutoUpdater.DAL.ExternalCommands.ExternalCommandRunner).ToString();
								Debug.WriteLine(ExternalCommandRunner);
												// IwAutoUpdater.DAL.LocalFiles, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
							var Directory = typeof(IwAutoUpdater.DAL.LocalFiles.Directory).ToString();
								Debug.WriteLine(Directory);
												// IwAutoUpdater.DAL.Notifications, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
							var MailReceiver = typeof(IwAutoUpdater.DAL.Notifications.MailReceiver).ToString();
								Debug.WriteLine(MailReceiver);
												// IwAutoUpdater.DAL.Updates, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
							var DefaultUpdatePackage = typeof(IwAutoUpdater.DAL.Updates.DefaultUpdatePackage).ToString();
								Debug.WriteLine(DefaultUpdatePackage);
												// IwAutoUpdater.DAL.WebAccess, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
							var HtmlGetter = typeof(IwAutoUpdater.DAL.WebAccess.HtmlGetter).ToString();
								Debug.WriteLine(HtmlGetter);
						        }
    }
}