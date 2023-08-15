using Microsoft.Extensions.Logging;
using ProjetoTransactionQueue.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace ProjetoTransactionQueue.Services
{
    public class SendQueueService : ISendQueueService
    {
        private IRabbitMqService _rabbitMqService;
        private ILogger<SendQueueService> _logger;
        public SendQueueService(IRabbitMqService rabbitMqService, ILogger<SendQueueService> logger) 
        {
            _rabbitMqService = rabbitMqService;
            _logger = logger;
        }
        public async Task SendTransaction(string transactionId)
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

            _logger.LogInformation($"{DateTime.Now} | Transaction has send to queue");
        }
    }
}
