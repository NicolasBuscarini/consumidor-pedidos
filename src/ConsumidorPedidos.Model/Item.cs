using System.Text.Json.Serialization;

namespace ConsumidorPedidos.Model
{
    /// <summary>
    /// Represents an item with product details, quantity, and price.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Gets or sets the unique identifier for the item.
        /// This property is ignored during JSON serialization.
        /// </summary>
        [JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// This property is serialized as "produto" in JSON.
        /// </summary>
        [JsonPropertyName("produto")]
        public required string Product { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product.
        /// This property is serialized as "quantidade" in JSON.
        /// </summary>
        [JsonPropertyName("quantidade")]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the price of the product.
        /// This property is serialized as "preco" in JSON.
        /// </summary>
        [JsonPropertyName("preco")]
        public float Price { get; set; }
    }
}
