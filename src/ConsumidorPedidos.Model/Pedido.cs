using System.ComponentModel.DataAnnotations;

namespace ConsumidorPedidos.Model
{
    public class Pedido
    {
        [Key]
        public int CodigoPedido { get; set; }
        public int CodigoCliente { get; set; }
        public required Item[] Itens { get; set; }
    }
}
