
using ProjetoTransactionQueue.Dto;
using ProjetoTransactionQueue.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ProjetoTransactionQueue.Services
{
    public class ReceiveQueueService : IReceiveQueueService
    {
        private readonly IModel _model;
        private readonly IConnection _connection;
        private readonly ITransactionFundService _transactionFundService;
        public ReceiveQueueService(IRabbitMqService rabbitMqService, ITransactionFundService transactionFundService)
        {
            _connection = rabbitMqService.CreateChannel();
            _model = _connection.CreateModel();
            _transactionFundService = transactionFundService;
        }

        public async Task Receive()
        {
            _model.QueueDeclare("transaction", durable: true, exclusive: false, autoDelete: false);
            _model.ExchangeDeclare("UserExchange", ExchangeType.Fanout, durable: true, autoDelete: false);
            _model.QueueBind("transaction", "UserExchange", string.Empty);
            var consumer = new AsyncEventingBasicConsumer(_model);
            consumer.Received += async (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var text = System.Text.Encoding.UTF8.GetString(body);
                var response = await _transactionFundService.UpdateTransaction(text);
                if (response)
                {
                    await Task.CompletedTask;
                    _model.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    await Task.CompletedTask;
                    _model.BasicReject(ea.DeliveryTag, true);
                }
            };
            _model.BasicConsume("transaction", false, consumer);
            await Task.CompletedTask;
        }
    }
}
