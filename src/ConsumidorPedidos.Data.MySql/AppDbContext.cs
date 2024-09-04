using ConsumidorPedidos.Model;
using Microsoft.EntityFrameworkCore;

namespace ConsumidorPedidos.Data.MySql
{
    /// <summary>
    /// Database context for the application.
    /// </summary>
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Item> Itens { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pedido>()
                .HasKey(p => p.CodigoPedido);

            modelBuilder.Entity<Item>()
                .HasKey(i => i.CodigoItem);
        }
    }
}
