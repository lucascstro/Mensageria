using System.ComponentModel.DataAnnotations;
using Pedido_api.Entidades;

namespace Pedido_api.Model
{
    public class PedidoRequest
    {
        [Required]
        public required string Id { get; set; }
        [Required]
        public required Cliente Cliente { get; set; }
        [Required]
        public required List<Produto> Produtos { get; set; }
    }
}