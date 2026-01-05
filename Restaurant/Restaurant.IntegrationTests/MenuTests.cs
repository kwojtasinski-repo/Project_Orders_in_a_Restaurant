using NUnit.Framework;
using Restaurant.IntegrationTests.Common;
using Restaurant.Shared.DTO;
using Shouldly;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Restaurant.IntegrationTests
{
    [TestFixture]
    public class MenuTests : BaseTest
    {
        private const string ApiBaseUrl = "/api/menu";

        [Test]
        public async Task GetMenu_ShouldReturnMenu()
        {
            // Arrange Act
            var response = await Client.GetAsync(ApiBaseUrl);

            // Assert
            response.EnsureSuccessStatusCode();
            var menu = await response.Content.ReadFromJsonAsync<MenuDto>();
            menu.ShouldNotBeNull();
            menu.Products.ShouldNotBeNull().ShouldNotBeEmpty();
            menu.Additions.ShouldNotBeNull().ShouldNotBeEmpty();
        }
    }
}
