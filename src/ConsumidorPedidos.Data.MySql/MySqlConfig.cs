using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ConsumidorPedidos.Data.MySql
{
    /// <summary>
    /// Class for configuring the database.
    /// </summary>
    public static class MySqlConfig
    {
        /// <summary>
        /// Configures the database with the specified SQLServer connection string.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="mySqlConnection">The MySql connection string.</param>
        public static void ConfigureDatabaseMySql(this IServiceCollection services, string mySqlConnection)
        {
            services.AddDbContextPool<AppDbContext>(options =>
            options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

        }

        /// <summary>
        /// Updates the database migration if there are pending migrations.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void UpdateMigrationDatabaseMySql(this IServiceCollection services)
        {
            // Configure database migration
            using var scope = services.BuildServiceProvider().CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            if (dbContext.Database.GetPendingMigrations().Any())
            {
                dbContext.Database.Migrate();
            }
        }
    }
}
