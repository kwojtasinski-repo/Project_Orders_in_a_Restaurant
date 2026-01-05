using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.Infrastructure.Barier
{
    public sealed class StartupBarrier : IStartupBarrier
    {
        private readonly TaskCompletionSource _tcs =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        public Task WaitAsync(CancellationToken ct)
            => _tcs.Task.WaitAsync(ct);

        public void SignalReady()
            => _tcs.TrySetResult();
    }
}
