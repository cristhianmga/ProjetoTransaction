using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoTransactionQueue.Interfaces
{
    public interface ITransactionFundService
    {
        Task<bool> UpdateTransaction(string transactionId);
    }
}
