

namespace ProjetoTransactionApplication.Interfaces
{
    public interface IApiAccountService
    {
        Task<bool> VerifyAccountNumber(string account);
        Task<bool> VerifyBalance(string account, decimal value);
    }
}
