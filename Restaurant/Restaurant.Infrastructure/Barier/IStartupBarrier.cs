using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.Infrastructure.Barier
{
    public interface IStartupBarrier
    {
        Task WaitAsync(CancellationToken ct);
        void SignalReady();
    }
}
