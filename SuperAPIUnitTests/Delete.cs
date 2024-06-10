using Microsoft.AspNetCore.Mvc;
using SuperAPI.Controllers;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.EntityFrameworkCore;
using SuperAPI.Entities;
using SuperAPITests;

namespace SuperAPITests
{
    public class DeleteHeroTests : SuperHeroControllerTestBase
    {
        private readonly SuperHeroController _controller;

        public DeleteHeroTests()
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
        public async Task DeleteHero_DeletesExistingHero()
        {
            // Arrange
            var heroToDelete = await _context.SuperHeroes.FirstAsync();

            // Act
            var result = await _controller.DeleteHero(heroToDelete.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var heroes = Assert.IsType<List<SuperHero>>(okResult.Value);
            var heroFromDb = await _context.SuperHeroes.FindAsync(heroToDelete.Id);
            Assert.Null(heroFromDb);
        }

        [Fact]
        public async Task DeleteHero_ReturnsNotFound_WhenHeroDoesNotExist()
        {
            // Arrange
            var nonExistingHeroId = 999; // Non-existing hero ID

            // Act
            var result = await _controller.DeleteHero(nonExistingHeroId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }
}
