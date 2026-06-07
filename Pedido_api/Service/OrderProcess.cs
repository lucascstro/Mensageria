using WorkerService;

namespace Pedido_api.Service
{
    public class OrderProcess
    {
        private readonly ILogger<OrderProcess> _logger;
        public OrderProcess(ILogger<OrderProcess> logger) => _logger = logger;
        public async Task SendMsgAsync(string queueName, string message)
        {
            try
            {
                WorkerProducer wp = new WorkerProducer();
                await wp.SendMsgAsync(queueName, message);

                Console.WriteLine($"Message was sended: {message}");
            }
            catch (Exception ex)
            {
                var msg = $"Error publishing message: {ex.Message}";
                _logger.LogWarning(msg);
            }
        }
    }
}