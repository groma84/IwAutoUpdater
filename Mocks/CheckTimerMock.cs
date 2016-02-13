using IwAutoUpdater.BLL.CommandPlanner.Contracts;

namespace Mocks
{
    public class CheckTimerMock : ICheckTimer
    {
        public bool IsCheckForUpdatesNecessary = false;
        public int IsCheckForUpdatesNecessaryCalled = 0;
        bool ICheckTimer.IsCheckForUpdatesNecessary(int checkIntervalMinutes)
        {
            ++IsCheckForUpdatesNecessaryCalled;
            return IsCheckForUpdatesNecessary;
        }
    }
}
