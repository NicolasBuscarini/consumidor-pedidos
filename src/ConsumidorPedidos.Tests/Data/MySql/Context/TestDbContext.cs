using ConsumidorPedidos.Data.MySql;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ConsumidorPedidos.Tests.Data.MySql.Context
{
    [ExcludeFromCodeCoverage]
    public class TestDbContext(DbContextOptions<AppDbContext> options) : AppDbContext(options)
    {
        public DbSet<TestEntity> TestEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure the primary key is configured
            modelBuilder.Entity<TestEntity>()
                .HasKey(te => te.Id);
        }
    }
}
