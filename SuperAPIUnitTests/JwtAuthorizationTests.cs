using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SuperAPITests
{
    public class JwtAuthorizationTests : IClassFixture<WebApplicationFactory<SuperAPI.Program>>
    {
        private readonly WebApplicationFactory<SuperAPI.Program> _factory;

        public JwtAuthorizationTests(WebApplicationFactory<SuperAPI.Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Konfiguracja testowa, jeśli jest potrzebna
                });
            });
        }

        [Fact]
        public async Task GetAllHeroes_ReturnsUnauthorized_WithoutToken()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/SuperHero");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetAllHeroes_ReturnsOk_WithValidToken()
        {
            // Arrange
            var client = _factory.CreateClient();
            var token = GenerateJwtToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await client.GetAsync("/api/SuperHero");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("SuperHero", responseString); // Sprawdź czy w odpowiedzi są dane
        }

        private string GenerateJwtToken()
        {
            // Prosty token JWT zawierający tylko wymagane pola dla celów testowych
            return "your_test_jwt_token";
        }
    }
}
