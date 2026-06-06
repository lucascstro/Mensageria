using RabbitMQ.Client;
using RabbitMQ.Client.Events;
namespace WorkerService;

public class WorkerReciver
{
    IConnection connection;
    IChannel channel;

    public async Task CreateConnectionAsync()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        connection = await factory.CreateConnectionAsync();
        channel = await connection.CreateChannelAsync();        
    }

    public async Task StartListeningAsync(string queueName)
    {
        var consumer = new AsyncEventingBasicConsumer(channel);
                
        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = System.Text.Encoding.UTF8.GetString(body);
            Console.WriteLine("Received: {0}", message);
            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(queueName, autoAck: true, consumer: consumer);
    }
}
