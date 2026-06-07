using System.Text.Json;
using WorkerService;

namespace Entrega_app;

class Program
{
    static async Task Main(string[] args)
    {
        var bw = new BaseWorker();
        await bw.CreateConnectionAsync("delivery");
        
        var ws = new WorkerService.WorkerReciver();
        Console.WriteLine("Delivery worker started.");
        Console.WriteLine("Acompanhando fila delivery...");

        await ws.StartListeningAsync("delivery", async (message) =>
        {
            Console.WriteLine("Solicitação de Entrega recebida, executandoo...");
            var msgDesserializada = JsonSerializer.Deserialize<dynamic>(message);
            Console.WriteLine($"Pedido: {msgDesserializada.GetProperty("Id")}");
            await Task.Delay(5000);
            Console.WriteLine("Entrega preparada, enviando equipe");

            await Task.Delay(2000);
            Console.WriteLine("Solicitação de entrega processada");
            return string.Empty;
        });

        await Task.Delay(-1);
    }
}