using Microsoft.EntityFrameworkCore;
using ProjetoTransactionApplication.Interfaces;
using ProjetoTransactionApplication.Services;
using ProjetoTransactionDomain.Repositories;
using ProjetoTransactionInfraData.Context;
using ProjetoTransactionInfraData.Repositories;
using ProjetoTransactionQueue.Interfaces;
using ProjetoTransactionQueue.Services;

namespace ProjetoTransactionApi.Configurations
{
    public static class AddRabbitMqService
    {
        public static IServiceCollection AddRabbitMqServiceInfra(this IServiceCollection services)
        {
            services.AddSingleton<IRabbitMqService, RabbitMqService>();
            return services;
        }
    }
}
