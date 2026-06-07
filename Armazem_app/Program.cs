using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using WorkerService;

namespace Armazem_app;

class Program
{
    static async Task Main(string[] args)
    {
        var ws = new WorkerReciver();
        var wp = new WorkerProducer();

        Console.WriteLine("storage worker started.");
        Console.WriteLine("Acompanhando fila storage...");

        await ws.StartListeningAsync("storage", async (message) =>
        {
            Console.WriteLine("Solicitação de separação recebida, executandoo...");
            var msgDesserializada = JsonSerializer.Deserialize<dynamic>(message);
            Console.WriteLine($"Pedido: {msgDesserializada.GetProperty("Id")}");
            await Task.Delay(5000);
            Console.WriteLine("Separação realizada");

            await Task.Delay(2000);

            Console.WriteLine("Notificando Entrega");
            await wp.SendMsgAsync("delivery", message);
        
            Console.WriteLine("Entrega Notificado.");

            await Task.Delay(2000);
            return "Solicitação de separação processado";
        });

        await Task.Delay(-1);
    }
}