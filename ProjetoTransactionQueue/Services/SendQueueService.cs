using ProjetoTransactionQueue.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace ProjetoTransactionQueue.Services
{
    public class SendQueueService : ISendQueueService
    {
        private IRabbitMqService _rabbitMqService;
        public SendQueueService(IRabbitMqService rabbitMqService) 
        {
            _rabbitMqService = rabbitMqService;
        }
        public async Task SendTransaction(string transactionId)
        {
            try
            {
                using var connection = _rabbitMqService.CreateChannel();
                using var model = connection.CreateModel();
                model.QueueDeclare(queue: "transaction",
                         durable: true,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

                var body = Encoding.UTF8.GetBytes(transactionId);
                model.BasicPublish(exchange: string.Empty,
                         routingKey: "transaction",
                         basicProperties: null,
                         body: body);
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
