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
    public class GetAllHeroesTests : SuperHeroControllerTestBase
    {
        private readonly SuperHeroController _controller;

        public GetAllHeroesTests()
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
        public async Task GetAllHeroes_ReturnsAllHeroes()
        {
            // Act
            var result = await _controller.GetAllHeroes();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var heroes = Assert.IsType<List<SuperHero>>(okResult.Value);
            Assert.Equal(4, heroes.Count);
        }
    }
}
