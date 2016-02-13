namespace IwAutoUpdater.BLL.CommandPlanner.Contracts
{
    public interface ICheckTimer
    {
        bool IsCheckForUpdatesNecessary(int checkIntervalMinutes);
    }
}
