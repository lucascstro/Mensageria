using System.Text;
using RabbitMQ.Client;

namespace WorkerService
{
    public class WorkerProducer
    {
        public async Task SendMsgAsync(string queueName, string message)
        {
            try
            {
                BaseWorker bs = new BaseWorker();
                var ret = await bs.CreateConnectionAsync(queueName);

                try
                {
                    var body = Encoding.UTF8.GetBytes(message);
                    await ret.BasicPublishAsync(exchange: string.Empty, routingKey: queueName, body: body);
                    
                    Console.WriteLine($"Message was sended: {body}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error publishing message: {ex.Message}");
                }
            }
            catch(Exception ex)
            {
                
            }
        }
    }
}