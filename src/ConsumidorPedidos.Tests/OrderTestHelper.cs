using ConsumidorPedidos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumidorPedidos.Tests
{
    public static class OrderTestHelper
    {
        public static readonly Item Item = new()
        {
            Product = "Lapis",
            Price = 10.0f,
            Quantity = 2,
            Id = 1
        };
        public static readonly Item Item2 = new()
        {
            Product = "Borracha",
            Price = 8.0f,
            Quantity = 1,
            Id = 1
        };
        public static readonly Order Order = new()
        {
            Id = 1,
            ClientCode = 2,
            Items = [Item, Item2]
        };
    }
}
