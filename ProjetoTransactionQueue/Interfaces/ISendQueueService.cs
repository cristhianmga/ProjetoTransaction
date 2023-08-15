

namespace ProjetoTransactionQueue.Interfaces
{
    public interface ISendQueueService
    {
        Task SendTransaction(string transactionId);
    }
}
