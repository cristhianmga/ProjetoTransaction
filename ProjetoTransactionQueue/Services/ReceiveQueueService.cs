
using Microsoft.Extensions.Logging;
using ProjetoTransactionQueue.Dto;
using ProjetoTransactionQueue.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ProjetoTransactionQueue.Services
{
    public class ReceiveQueueService : IReceiveQueueService, IDisposable
    {
        private readonly IModel _model;
        private readonly IConnection _connection;
        private readonly ITransactionFundService _transactionFundService;
        private readonly ILogger _logger;
        public ReceiveQueueService(IRabbitMqService rabbitMqService, ITransactionFundService transactionFundService, ILogger<ReceiveQueueService> logger)
        {
            _connection = rabbitMqService.CreateChannel();
            _model = _connection.CreateModel();
            _transactionFundService = transactionFundService;
            _logger = logger;
        }

        public async Task Receive()
        {
            _model.QueueDeclare("transaction", durable: true, exclusive: false, autoDelete: false);
            _model.ExchangeDeclare("UserExchange", ExchangeType.Fanout, durable: true, autoDelete: false);
            _model.QueueBind("transaction", "UserExchange", string.Empty);
            var consumer = new AsyncEventingBasicConsumer(_model);
            consumer.Received += async (ch, ea) =>
            {

                _logger.LogInformation($"{DateTime.Now} | Get queue transaction");
                var body = ea.Body.ToArray();
                var text = System.Text.Encoding.UTF8.GetString(body);
                var response = await _transactionFundService.UpdateTransaction(text);
                if (response)
                {
                    _logger.LogInformation($"{DateTime.Now} | Transaction has Unqueue");
                    await Task.CompletedTask;
                    _model.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    _logger.LogInformation($"{DateTime.Now} | Transaction has Requeue");
                    await Task.CompletedTask;
                    _model.BasicReject(ea.DeliveryTag, true);
                }
            };
            _model.BasicConsume("transaction", false, consumer);
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            if (_model.IsOpen)
                _model.Close();
            if (_connection.IsOpen)
                _connection.Close();
        }
    }
}
