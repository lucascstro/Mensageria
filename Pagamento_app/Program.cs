using System.Text.Json;
using WorkerService;

namespace Pagamento_app;

class Program
{
    static async Task Main(string[] args)
    {
        var ws = new WorkerReciver();
        var wp = new WorkerProducer();

        Console.WriteLine("Payment worker started.");
        Console.WriteLine("Acompanhando fila payment...");

        await ws.StartListeningAsync("payment", async (message) =>
        {
            Console.WriteLine("Solicitação de pagamento recebida, executandoo...");
            var msgDesserializada = JsonSerializer.Deserialize<dynamic>(message);
            Console.WriteLine($"Pedido: {msgDesserializada.GetProperty("Id")}");
            
            await Task.Delay(5000);
            Console.WriteLine("Pagamento realizado");

            await Task.Delay(2000);

            Console.WriteLine("Notificando Storage");
            await wp.SendMsgAsync("storage", message);
            Console.WriteLine("Storage Notificado.");

            await Task.Delay(2000);
            return "Pagamento processado";
        });
        
        await Task.Delay(-1);
    }
}