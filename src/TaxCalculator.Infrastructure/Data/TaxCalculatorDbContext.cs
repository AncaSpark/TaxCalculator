using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using TaxCalculator.Domain.Models;

namespace TaxCalculator.Infrastructure.Data
{
    public class TaxCalculatorDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public TaxCalculatorDbContext(DbContextOptions<TaxCalculatorDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        public DbSet<TaxBand> TaxBands { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations from assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Seed initial data
            modelBuilder.Entity<TaxBand>().HasData(
                new TaxBand
                {
                    Id = 1,
                    Name = "Tax Band A",
                    LowerLimit = 0,
                    UpperLimit = 5000,
                    TaxRate = 0
                },
                new TaxBand
                {
                    Id = 2,
                    Name = "Tax Band B",
                    LowerLimit = 5000,
                    UpperLimit = 20000,
                    TaxRate = 20
                },
                new TaxBand
                {
                    Id = 3,
                    Name = "Tax Band C",
                    LowerLimit = 20000,
                    UpperLimit = null,
                    TaxRate = 40
                }
            );
        }
    }
}