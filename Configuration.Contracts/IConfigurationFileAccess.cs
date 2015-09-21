namespace IwAutoUpdater.CrossCutting.Configuration.Contracts
{
    public interface IConfigurationFileAccess
    {
        string ReadAllText(string path);
    }
}