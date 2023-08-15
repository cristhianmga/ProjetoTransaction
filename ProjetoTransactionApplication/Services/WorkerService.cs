using AutoMapper;
using Microsoft.Extensions.Logging;
using ProjetoTransactionApplication.Interfaces;
using ProjetoTransactionDomain.Entities;
using ProjetoTransactionDomain.Repositories;
using ProjetoTransactionQueue.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoTransactionApplication.Services
{
    public class WorkerService : IWorkedService
    {

        private readonly ILogger<WorkerService> _logger;
        private readonly IApiAccountService _apiAccountService;
        private readonly IRepositoryBase<Transaction> _repository;
        private readonly IMapper _mapper;

        public WorkerService(ILogger<WorkerService> logger, IApiAccountService apiAccountService, IRepositoryBase<Transaction> repository, IMapper mapper)
        {
            _logger = logger;
            _apiAccountService = apiAccountService;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> TransactExecute(string transactionId)
        {
            bool executou = false;
            await UpdateTransactBd(transactionId,Enum.TransactionStatus.Processing);
            //atualizar os valores das contas.
            if (executou)
            {
                await UpdateTransactBd(transactionId, Enum.TransactionStatus.Confirmed);
            }
            else
            {
                await UpdateTransactBd(transactionId, Enum.TransactionStatus.InQueue);
            }
            return executou;
        }

        private async Task UpdateTransactBd(string transactionId,Enum.TransactionStatus status)
        {
            _logger.LogInformation($"{DateTime.Now} | Initiate update Transaction status");
            var response = await _repository.GetByIdAsync(new Guid(transactionId));
            response.Status = (int)status;
            await _repository.UpdateAsync(response);
            await _repository.SaveChangesAsync();
            _logger.LogInformation($"{DateTime.Now} | Ending update Transaction status");
        }
    }
}
