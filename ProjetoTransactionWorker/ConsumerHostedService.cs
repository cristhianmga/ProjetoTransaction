
using Microsoft.Extensions.Hosting;
using ProjetoTransactionQueue.Interfaces;

namespace ProjetoTransactionWorker
{
    public class ConsumerHostedService : BackgroundService
    {
        private readonly IReceiveQueueService _consumerService;

        public ConsumerHostedService(IReceiveQueueService consumerService)
        {
            _consumerService = consumerService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumerService.Receive();
        }
    }
}
