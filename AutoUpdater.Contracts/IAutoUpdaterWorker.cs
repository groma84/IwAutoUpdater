using System.Threading.Tasks;

namespace IwAutoUpdater.BLL.AutoUpdater.Contracts
{
    public interface IAutoUpdaterWorker
    {
        Task NeverendingWorkLoop();
    }
}
