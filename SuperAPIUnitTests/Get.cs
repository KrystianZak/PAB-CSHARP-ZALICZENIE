using Microsoft.AspNetCore.Mvc;
using SuperAPI.Controllers;
using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.EntityFrameworkCore;
using SuperAPI.Entities;
using SuperAPITests;

namespace SuperAPITests
{
    public class GetHeroTests : SuperHeroControllerTestBase
    {
        private readonly SuperHeroController _controller;

        public GetHeroTests()
        {
            _controller = new SuperHeroController(_context);

            // Mock policy to bypass [Authorize] attribute during tests
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    RequestServices = new ServiceCollection()
                        .AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>()
                        .BuildServiceProvider()
                }
            };
        }

        [Fact]
        public async Task GetHero_ReturnsHero_WhenHeroExists()
        {
            // Arrange
            var heroId = 1; // Assuming the first hero has ID 1

            // Act
            var result = await _controller.GetHero(heroId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var hero = Assert.IsType<SuperHero>(okResult.Value);
            Assert.Equal(heroId, hero.Id);
        }

        [Fact]
        public async Task GetHero_ReturnsBadRequest_WhenHeroDoesNotExist()
        {
            // Arrange
            var heroId = 999; // Non-existing hero ID

            // Act
            var result = await _controller.GetHero(heroId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }
}
