namespace Pagamento_app;

class Program
{
    static async Task Main(string[] args)
    {
        var ws = new WorkerService.WorkerReciver();
        Console.WriteLine("Worker started.");
        await ws.CreateConnectionAsync();
        Console.WriteLine("Connection started.");
        
        Console.WriteLine("Acompanhando fila payment...");
        while (true)
        {
            await ws.StartListeningAsync("payment");
            await Task.Delay(1000);
        }
    }
}
