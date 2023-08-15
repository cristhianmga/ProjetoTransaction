using ProjetoTransactionQueue.Interfaces;
using RabbitMQ.Client;

namespace ProjetoTransactionQueue.Services
{
    public class RabbitMqService : IRabbitMqService
    {
        public IConnection CreateChannel() 
        {
            ConnectionFactory connection = new ConnectionFactory()
            {
                HostName = "localhost"
            };
            connection.DispatchConsumersAsync = true;
            var channel = connection.CreateConnection();
            return channel;
        }
    }
}
