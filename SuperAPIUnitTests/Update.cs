using Microsoft.EntityFrameworkCore;
using SuperAPI.Data;
using SuperAPI.Entities;
using System;

namespace SuperHeroTests
{
    public class SuperHeroControllerTestBase : IDisposable
    {
        protected readonly DataContext _context;

        public SuperHeroControllerTestBase()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new DataContext(options);
            _context.Database.EnsureCreated();

            // Ensure database is empty before adding new data
            _context.SuperHeroes.RemoveRange(_context.SuperHeroes);
            _context.SaveChanges();

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
    }
}
