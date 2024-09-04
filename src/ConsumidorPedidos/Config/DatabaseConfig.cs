using ConsumidorPedidos.Data.MySql;

namespace ConsumidorPedidos.Config
{
    /// <summary>
    /// Class for configuring the database.
    /// </summary>
    public static class DatabaseConfig
    {
        /// <summary>
        /// Configures the database with the specified connection string.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="sqlConnection">The connection string.</param>
        public static void ConfigureDatabase(this IServiceCollection services, string sqlConnection)
        {
            services.ConfigureDatabaseMySql(sqlConnection);
        }

        /// <summary>
        /// Updates the database migration if there are pending migrations.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void UpdateMigrationDatabase(this IServiceCollection services)
        {
            services.UpdateMigrationDatabaseMySql();
        }
    }
}
