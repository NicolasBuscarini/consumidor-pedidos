using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ConsumidorPedidos.Data.MySql
{
    /// <summary>
    /// Database context for the application.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </remarks>
    /// <remarks>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </remarks>
    /// <param name="options">The options for this context.</param>

    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
