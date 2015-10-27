using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using System.Threading.Tasks;

namespace IwAutoUpdater.BLL.AutoUpdater.Contracts
{
    public interface IAutoUpdaterThreadFactory
    {
        void CreateAndRunEndlessLoops(Settings settings);
    }
}
