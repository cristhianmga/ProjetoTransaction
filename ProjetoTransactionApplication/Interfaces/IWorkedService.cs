using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoTransactionApplication.Interfaces
{
    public interface IWorkedService
    {
        Task<bool> TransactExecute(string transactionId);
    }
}
