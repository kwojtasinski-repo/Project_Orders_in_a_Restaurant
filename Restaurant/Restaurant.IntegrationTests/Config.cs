using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Restaurant.IntegrationTests.Common;
using System;
using System.IO;

namespace Restaurant.IntegrationTests
{
    [SetUpFixture]
    public class Config
    {
        public static IServiceProvider Container;
        private static IServiceScope serviceScope;

        [OneTimeSetUp]
        public void OnetTimeSetup()
        {
            Container = new TestApplicationFactory().StartApplication();
            serviceScope = Container.CreateScope();
        }

        [OneTimeTearDown]
        public void OneTimeTeardown()
        {
            System.Data.SQLite.SQLiteConnection.ClearAllPools();
            serviceScope?.Dispose();
            if (Container is IDisposable disposable)
            {
                disposable.Dispose();
            }

            File.Delete(Environment.CurrentDirectory + Path.DirectorySeparatorChar + TestApplicationFactory.DB_FILE_NAME);
        }
    }
}
