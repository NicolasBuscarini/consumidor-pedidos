using System.Diagnostics.CodeAnalysis;

namespace ConsumidorPedidos.Tests.Data.MySql
{
    [ExcludeFromCodeCoverage]
    public class TestEntity
    {
        public int Id { get; set; }
        public string? SomeProperty { get; set; }
    }
}
