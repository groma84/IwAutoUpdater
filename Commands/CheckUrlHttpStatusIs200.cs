using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.DAL.Updates.Contracts;
using IwAutoUpdater.DAL.WebAccess.Contracts;

namespace IwAutoUpdater.BLL.Commands
{
    public class CheckUrlHttpStatusIs200 : Command
    {
        private readonly ProxySettings _proxySettings;
        private readonly IHtmlGetter _htmlGetter;
        private readonly string _url;
        private readonly IUpdatePackage _package;

        public CheckUrlHttpStatusIs200(string url, IUpdatePackage package, IHtmlGetter htmlGetter, ProxySettings proxySettings = null)
        {
            _url = url;

            _package = package;
            _htmlGetter = htmlGetter;
            _proxySettings = proxySettings;
        }

        public override string PackageName
        {
            get
            {
                return _package.PackageName;
            }
        }

        public override CommandResult Do(CommandResult resultOfPreviousCommand)
        {
            var htmlResult = _htmlGetter.DownloadHtml(_url, _proxySettings);

            if (htmlResult.HttpStatusCode != 200)
            {
                return new CommandResult(false, new[] { new Error() { Text = htmlResult.Content, Exception = null } });
            }
            else
            {
                return new CommandResult(true);
            }
        }
    }
}
