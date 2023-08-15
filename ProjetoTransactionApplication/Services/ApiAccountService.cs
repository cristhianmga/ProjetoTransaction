using Microsoft.Extensions.Logging;
using ProjetoTransactionApplication.Dtos;
using ProjetoTransactionApplication.Interfaces;
using System.Text.Json;

namespace ProjetoTransactionApplication.Services
{
    public class ApiAccountService : IApiAccountService
    {
        private readonly ILogger<ApiAccountService> _logger;
        private readonly HttpClient _httpClient;

        public ApiAccountService(ILogger<ApiAccountService> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("ApiAccount");
        }

        public async Task<bool> VerifyAccountNumber(string account)
        {
            var response = await _httpClient.GetAsync("/api/Account/" + account);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) 
            {
                return false;
            }

            return true;
        }

        public async Task<bool> VerifyBalance(string account,decimal value)
        {
            var response = await _httpClient.GetAsync("/api/Account/" + account);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            AccountDto accountDtoResponse = JsonSerializer.Deserialize<AccountDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (accountDtoResponse.Balance < value)
            {
                return false;
            }

            return true;
        }
    }
}
