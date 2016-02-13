using IwAutoUpdater.CrossCutting.Base;
using IwAutoUpdater.DAL.Updates.Contracts;
using IwAutoUpdater.DAL.WebAccess.Contracts;
using IwAutoUpdater.CrossCutting.Logging.Contracts;

namespace IwAutoUpdater.BLL.Commands
{
    public class CheckUrlHttpStatusIs200 : Command
    {
        private readonly ILogger _logger;
        private readonly ProxySettings _proxySettings;
        private readonly IHtmlGetter _htmlGetter;
        private readonly string _url;
        private readonly IUpdatePackage _package;

        public CheckUrlHttpStatusIs200(string url, IUpdatePackage package, IHtmlGetter htmlGetter, ILogger logger, ProxySettings proxySettings = null)
        {
            _url = url;

            _package = package;
            _htmlGetter = htmlGetter;
            _logger = logger;

            _proxySettings = proxySettings;
        }

        public override string PackageName
        {
            get
            {
                return _package.PackageName;
            }
        }

        public override CommandResult Do()
        {
            _logger.Debug("Trying to download html from {url}", _url);

            var htmlResult = _htmlGetter.DownloadHtml(_url, _proxySettings);

            _logger.Debug("Checking status code of html result for {url}", _url);

            if (htmlResult.HttpStatusCode != 200)
            {
                _logger.Debug("Status code is {statusCode}", htmlResult.HttpStatusCode);

                return new CommandResult(false, new[] { new Error() { Text = htmlResult.Content, Exception = null } });
            }
            else
            {
                return new CommandResult(true);
            }
        }

        public override Command Copy()
        {
            var x = new CheckUrlHttpStatusIs200(_url, _package, _htmlGetter, _logger, _proxySettings);
            x.RunAfterCompletedWithResultFalse = this.RunAfterCompletedWithResultFalse;
            x.RunAfterCompletedWithResultTrue = this.RunAfterCompletedWithResultTrue;
            x.AddResultsOfPreviousCommands(this.ResultsOfPreviousCommands);

            return x;
        }
    }
}
