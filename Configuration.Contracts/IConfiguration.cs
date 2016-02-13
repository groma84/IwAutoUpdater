namespace IwAutoUpdater.CrossCutting.Configuration.Contracts
{
    public interface IConfiguration
    {
        Settings Get(string location);
    }
}
