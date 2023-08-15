

using RabbitMQ.Client.Events;

namespace ProjetoTransactionQueue.Dto
{
    public class ReceiveQueueDto
    {
        public string Body { get; set; }
        public EventingBasicConsumer Consumer { get; set; }
    }
}
