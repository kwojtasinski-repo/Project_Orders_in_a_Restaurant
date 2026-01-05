using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurant.Infrastructure.Barier;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.Infrastructure.DAL.Initialization
{
    internal class DbInitializer(IServiceProvider serviceProvider, IStartupBarrier barrier) : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            migrationRunner.MigrateUp();
            barrier.SignalReady();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
