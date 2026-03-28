using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WarehouseStockService.Application.Ports;
using WarehouseStockService.Domain.Repositories;
using WarehouseStockService.Infrastructure.Messaging;
using WarehouseStockService.Infrastructure.Persistence;
using WarehouseStockService.Infrastructure.Repositories;

namespace WarehouseStockService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("ConnectionString 'Default' not found.");

        services.AddScoped<DbSession>(_ => new DbSession(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IBranchRepository, BranchRepository>();
        services.AddScoped<IStockTemplateRepository, StockTemplateRepository>();
        services.AddScoped<IStockLocationRepository, StockLocationRepository>();
        services.AddScoped<IItemLocationRepository, ItemLocationRepository>();
        services.AddScoped<IStockMovementRepository, StockMovementRepository>();

        var rabbitMqSettings = new RabbitMqSettings
        {
            Host = configuration["RabbitMq:Host"] ?? "localhost",
            Port = int.TryParse(configuration["RabbitMq:Port"], out var port) ? port : 5672,
            Username = configuration["RabbitMq:Username"] ?? "guest",
            Password = configuration["RabbitMq:Password"] ?? "guest",
            VirtualHost = configuration["RabbitMq:VirtualHost"] ?? "/",
            ExchangeName = configuration["RabbitMq:ExchangeName"] ?? "warehouse-stock"
        };

        services.AddSingleton(rabbitMqSettings);
        services.AddSingleton<RabbitMqPublisher>();
        services.AddSingleton<IMessagePublisher>(sp => sp.GetRequiredService<RabbitMqPublisher>());
        services.AddHostedService(sp => sp.GetRequiredService<RabbitMqPublisher>());

        return services;
    }
}
