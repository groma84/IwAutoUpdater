using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.CrossCutting.Logging.Contracts;

namespace IwAutoUpdater.CrossCutting.Configuration
{
    public class JsonFileConfiguration : IConfiguration
    {
        private readonly ILogger _logger;
        private readonly IConfigurationFileAccess _fileAccess;

        public JsonFileConfiguration(IConfigurationFileAccess fileAccess, ILogger logger)
        {
            _fileAccess = fileAccess;
            _logger = logger;
        }

        Settings IConfiguration.Get(string location)
        {
            _logger.Info("Loading configuration file from {Location}", location);

            var text = _fileAccess.ReadAllText(location);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<Settings>(text);
        }
    }
}
