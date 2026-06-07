using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Pedido_api.Model;
using Pedido_api.Service;

namespace Pedido_api.Controllers;

[ApiController]
[Route("[controller]")]
public class PedidoController : ControllerBase
{
    private readonly ILogger<PedidoController> _logger;
    private readonly OrderProcess _orderProcess;

    public PedidoController(ILogger<PedidoController> logger, OrderProcess orderProcess)
    {
        _logger = logger;
        _orderProcess = orderProcess;
    }

    [HttpPost(Name = "Pedido")]
    public async Task<IActionResult> GetPedido([FromBody] PedidoRequest pedido)
    {
        try
        {
            string msg = string.Empty;

            if (pedido.Produtos == null || pedido.Produtos.Count <= 0)
                msg += "O campo 'Quantidade' é obrigatório e deve ser um valor positivo. ";

            if (pedido.Cliente.Nome == null || pedido.Cliente.Nome.Trim() == "")
                msg += "O campo 'Cliente.Nome' é obrigatório. ";

            if (pedido.Cliente.Email == null || pedido.Cliente.Email.Trim() == "")
                msg += "O campo 'Cliente.Email' é obrigatório. ";

            if (pedido.Cliente.Endereco == null || pedido.Cliente.Endereco.Trim() == "")
                msg += "O campo 'Cliente.Endereco' é obrigatório. ";

            msg += $"Código do pedido: {pedido.Id} - Data do pedido: {DateTime.Now} \r\n";
            msg += $"Cliente: {pedido.Cliente.Nome} - Email: {pedido.Cliente.Email} - Endereço: {pedido.Cliente.Endereco} \r\n";

            foreach (var produto in pedido.Produtos)
                msg += $"Produto: {produto.Nome} - Quantidade: {produto.Quantidade} - Preço: {produto.Preco} \r\n";
                
            await _orderProcess.SendMsgAsync("payment", JsonSerializer.Serialize(pedido));

            return Ok("Pedido recebido com sucesso! \r\n" +
                      "Você recebera os detalhes de cada etapa email informado. \r\n" +
                      "Detalhes do pedido: \r\n" + msg);
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Something happened, we so sorry! Details: {ex.Message}");
            throw;
        }
    }
}