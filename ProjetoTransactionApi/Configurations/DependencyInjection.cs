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
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Db")));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));


            services.AddScoped<ITransferFundService, TransferFundService>();
            services.AddScoped<IApiAccountService, ApiAccountService>();
            services.AddScoped<IWorkedService, WorkerService>();
            services.AddSingleton<ITransactionFundService, TransactionFundService>();
            
            return services;
        }
    }
}
