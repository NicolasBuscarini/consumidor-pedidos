using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ConsumidorPedidos.Model
{
    /// <summary>
    /// Represents an order with a unique identifier, client code, and a list of items.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Gets or sets the unique identifier for the order.
        /// This property is serialized as "codigoPedido" in JSON.
        /// </summary>
        [Key]
        [JsonPropertyName("codigoPedido")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the client code associated with the order.
        /// This property is serialized as "codigoCliente" in JSON.
        /// </summary>
        [JsonPropertyName("codigoCliente")]
        public int ClientCode { get; set; }

        /// <summary>
        /// Gets or sets the items included in the order.
        /// This property is serialized as "itens" in JSON.
        /// </summary>
        [JsonPropertyName("itens")]
        public required List<Item> Items { get; set; } = [];

        /// <summary>
        /// Gets the total amount of the order, which is calculated based on the sum of the item prices.
        /// This property is not mapped to the database and is serialized as "total" in JSON.
        /// </summary>
        [NotMapped]
        [JsonPropertyName("total")]
        public decimal Total => Items.Sum(item => (decimal)item.Price * item.Quantity);
    }
}
