

using ProjetoTransactionApplication.Dtos;

namespace ProjetoTransactionApplication.Interfaces
{
    public interface ITransferFundService
    {
        Task<FundTransferResponseDto> ExecuteTransaction(FundTransferRequestDto request);
        Task<TransactionResponseDto> GetStatusByTransactionId(Guid transactionId);
    }
}
