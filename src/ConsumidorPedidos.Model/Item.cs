using System.Text.Json.Serialization;

namespace ConsumidorPedidos.Model
{
    public class Item
    {
        [JsonIgnore]
        public int CodigoItem { get; set; }
        public required string Produto { get; set; }
        public int Quantidade { get; set; }
        public float Preco { get; set; }
    }
}
