
using ProjetoTransactionQueue.Dto;

namespace ProjetoTransactionQueue.Interfaces
{
    public interface IReceiveQueueService
    {
        Task Receive();
    }
}
