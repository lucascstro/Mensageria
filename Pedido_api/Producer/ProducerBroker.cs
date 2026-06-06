using RabbitMQ.Client;
using System.Text;

namespace Pedido_api.Producer
{
    public class ProducerBroker
    {
        private readonly ILogger<ProducerBroker> _logger;
        private IChannel channel;
        private IConnection connection;

        public ProducerBroker(ILogger<ProducerBroker> logger)
        {
            _logger = logger;
        }

        public async Task CreatConnectionAsync()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            connection = await factory.CreateConnectionAsync();
            channel = await connection.CreateChannelAsync();
        }

        public async Task SendMsgAsync(string queueName, string message)
        {
            try
            {

                await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: new Dictionary<string, object?> { { "x-queue-type", "quorum" } });

                try
                {  
                    var body = Encoding.UTF8.GetBytes(message);
                    await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queueName, body: body);
                    Console.WriteLine($"Message was sended: {body}");

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error publishing message: {ex.Message}");
                }
                finally
                {
                    await channel.CloseAsync();
                }
            }
            finally
            {
                await channel.CloseAsync();
                await connection.CloseAsync();
            }
        }
    }
}