using Microsoft.Extensions.DependencyInjection;
using ProjetoTransactionApplication.Interfaces;
using ProjetoTransactionQueue.Interfaces;

namespace ProjetoTransactionApplication.Services
{
    public class TransactionFundService : ITransactionFundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public TransactionFundService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<bool> UpdateTransaction(string transactionId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var myScopedService = scope.ServiceProvider.GetService<IWorkedService>();
                return await myScopedService.TransactExecute(transactionId);
            }
        }
    }
}
