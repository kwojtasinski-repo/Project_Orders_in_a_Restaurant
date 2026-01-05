using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurant.Infrastructure.Barier;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.IntegrationTests.Common
{
    internal class InitializeDb(IServiceProvider serviceProvider, IStartupBarrier barrier) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await barrier.WaitAsync(cancellationToken);
            using var scope = serviceProvider.CreateScope();
            DataSeed.AddData(scope.ServiceProvider);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
