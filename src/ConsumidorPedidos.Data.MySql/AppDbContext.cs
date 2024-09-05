using ConsumidorPedidos.Model;
using Microsoft.EntityFrameworkCore;

namespace ConsumidorPedidos.Data.MySql
{
    /// <summary>
    /// Database context for the application.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class with the specified options.
        /// </summary>
        /// <param name="options">The options to be used by the <see cref="AppDbContext"/>.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the Orders table.
        /// </summary>
        public DbSet<Order> Order { get; set; }

        /// <summary>
        /// Gets or sets the Items table.
        /// </summary>
        public DbSet<Item> Item { get; set; }

        /// <summary>
        /// Configures the model that was discovered by convention from the entity types.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for the database.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
