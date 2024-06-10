using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperAPI.Controllers;
using SuperAPI.Data;
using SuperAPI.Entities;
using Xunit;

namespace SuperAPITests
{
    public class SuperHeroAddTests : IDisposable
    {
        private readonly DataContext _context;
        private readonly SuperHeroController _controller;

        public SuperHeroAddTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new DataContext(options);
            _context.Database.EnsureCreated();
            _controller = new SuperHeroController(_context);

            _context.SuperHeroes.AddRange(
                new SuperHero { Name = "Superman", FirstName = "Clark", LastName = "Kent", Place = "Metropolis" },
                new SuperHero { Name = "Batman", FirstName = "Bruce", LastName = "Wayne", Place = "Gotham" }
            );
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task AddHero_AddsNewHero()
        {
            // Arrange
            var newHero = new SuperHero { Name = "Wonder Woman", FirstName = "Diana", LastName = "Prince", Place = "Themyscira" };

            // Act
            var result = await _controller.AddHero(newHero);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var heroes = Assert.IsType<List<SuperHero>>(okResult.Value);
            var expectedCount = await _context.SuperHeroes.CountAsync();
            Assert.Equal(3, expectedCount); // Ponieważ dodajemy jednego bohatera do istniejących dwóch
            Assert.Contains(heroes, h => h.Name == "Wonder Woman" && h.FirstName == "Diana" && h.LastName == "Prince");
        }
    }
}
