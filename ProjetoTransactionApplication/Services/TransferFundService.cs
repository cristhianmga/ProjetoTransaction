
using AutoMapper;
using Microsoft.Extensions.Logging;
using ProjetoTransactionApplication.Dtos;
using ProjetoTransactionApplication.Interfaces;
using ProjetoTransactionDomain.Entities;
using ProjetoTransactionDomain.Repositories;
using ProjetoTransactionQueue.Interfaces;
using RabbitMQ.Client;

namespace ProjetoTransactionApplication.Services
{
    public class TransferFundService : ITransferFundService
    {
        private readonly ILogger<TransferFundService> _logger;
        private readonly IApiAccountService _apiAccountService;
        private readonly IRepositoryBase<Transaction> _repository;
        private readonly IMapper _mapper;
        private readonly ISendQueueService _sendQueueService;
        public TransferFundService(ILogger<TransferFundService> logger, IApiAccountService apiAccountService, IRepositoryBase<Transaction> repository,IMapper mapper, ISendQueueService sendQueueService) 
        {
            _logger = logger;
            _apiAccountService = apiAccountService;
            _repository = repository;
            _mapper = mapper;
            _sendQueueService = sendQueueService;
        }

        public async Task<FundTransferResponseDto> ExecuteTransaction(FundTransferRequestDto request)
        {
            bool verifyAccountOrigin = await _apiAccountService.VerifyAccountNumber(request.AccountOrigin);
            if(!verifyAccountOrigin)
            {
                return await CreateTransaction(request, Enum.TransactionStatus.Error, "Invalid Origin account number");
            }

            bool verifyAccountDestination = await _apiAccountService.VerifyAccountNumber(request.AccountDestination);
            if (!verifyAccountDestination)
            {
                return await CreateTransaction(request, Enum.TransactionStatus.Error, "Invalid Origin account number");
            }

            bool verifyBalance = await _apiAccountService.VerifyBalance(request.AccountOrigin, request.Value);
            if (!verifyBalance)
            {
                return await CreateTransaction(request, Enum.TransactionStatus.Error, "No funds");
            }

            return await CreateTransaction(request, Enum.TransactionStatus.InQueue);
        }


        private async Task<FundTransferResponseDto> CreateTransaction(FundTransferRequestDto request,Enum.TransactionStatus status, string error = null)
        {
            Transaction transaction = _mapper.Map<Transaction>(request);
            transaction.Status = (int)status;
            transaction.Error = error;
            var transactionBd = await _repository.AddAsync(transaction);
            await _repository.SaveChangesAsync();

            FundTransferResponseDto response = new FundTransferResponseDto()
            {
                TransactionId = transactionBd.Id
            };
            SendToQueue(transactionBd.Id.ToString());
            return response;
        }

        private async Task SendToQueue(string transactionId)
        {
            await _sendQueueService.SendTransaction(transactionId);
        }
    }
}
