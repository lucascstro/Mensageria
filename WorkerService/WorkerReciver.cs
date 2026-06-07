using RabbitMQ.Client;
using RabbitMQ.Client.Events;
namespace WorkerService;

public class WorkerReciver
{
    public async Task StartListeningAsync(string queueName, Func<string, Task<string>> processar)
    {
        BaseWorker bs = new BaseWorker();
        var ret = await bs.CreateConnectionAsync();

        var consumer = new AsyncEventingBasicConsumer(ret);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = System.Text.Encoding.UTF8.GetString(body);
            var resultProc = processar(message);
            return;
        };

        await ret.BasicConsumeAsync(queueName, autoAck: true, consumer: consumer);
    }
}