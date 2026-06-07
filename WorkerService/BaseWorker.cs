using RabbitMQ.Client;

namespace WorkerService
{
    public class BaseWorker
    {
        IConnection connection;
        IChannel channel;
        public async Task<IChannel> CreateConnectionAsync(string? queueName = null)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            connection = await factory.CreateConnectionAsync();
            channel = await connection.CreateChannelAsync();

            if( queueName is not null)
                await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: new Dictionary<string, object?> { { "x-queue-type", "quorum" } });

            return channel;
        }
    }
}