using ConsumidorPedidos.Core.Repository;
using ConsumidorPedidos.Data.MySql;
using ConsumidorPedidos.Tests.Data.MySql;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace ConsumidorPedidos.Tests.Core.Repository
{
    [ExcludeFromCodeCoverage]
    public class TestGenericRepository(AppDbContext context, ILogger<GenericRepository<TestEntity, int>> logger) : GenericRepository<TestEntity, int>(context, logger)
    {
    }
}
