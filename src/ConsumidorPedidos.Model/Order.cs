using System.ComponentModel.DataAnnotations;
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
        public required List<Item> Items { get; set; } = new List<Item>();
    }
}
