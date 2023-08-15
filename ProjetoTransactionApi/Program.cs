using Microsoft.EntityFrameworkCore;
using ProjetoTransactionApi.Configurations;
using ProjetoTransactionApi.Middlewares;
using ProjetoTransactionInfraData.Context;
using ProjetoTransactionQueue.Interfaces;
using ProjetoTransactionQueue.Services;
using ProjetoTransactionWorker;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHttpClient("ApiAccount", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BaseUrlApiAccount"]);
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddRabbitMqServiceInfra();
builder.Services.AddSingleton<IReceiveQueueService, ReceiveQueueService>();
builder.Services.AddSingleton<ISendQueueService, SendQueueService>();
builder.Services.AddHostedService<ConsumerHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApplicationDbContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

app.Run();
