using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Net.Http;

namespace Restaurant.IntegrationTests.Common
{
    public abstract class BaseTest
    {
        private TestApplicationFactory _factory = null!;
        private IServiceScope _scope;
        protected HttpClient Client = null!;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _factory = new TestApplicationFactory();
            Client = _factory.CreateClient();
        }

        [SetUp]
        public void BaseSetup()
        {
            _scope = _factory.Services.CreateScope();
        }

        [TearDown]
        public void BaseTeardown()
        {
            _scope?.Dispose();
            _scope = null;
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Client.Dispose();
            _factory.Dispose();
        }

        public T GetService<T>()
        {
            return _scope.ServiceProvider.GetRequiredService<T>();
        }
    }
}
