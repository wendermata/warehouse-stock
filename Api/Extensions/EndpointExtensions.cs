using WarehouseStockService.Endpoints;

namespace WarehouseStockService.Extensions;

public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        services.AddTransient<IEndpoint, BranchEndpoints>();
        services.AddTransient<IEndpoint, StockTemplateEndpoints>();
        services.AddTransient<IEndpoint, StockLocationEndpoints>();
        services.AddTransient<IEndpoint, ItemLocationEndpoints>();
        services.AddTransient<IEndpoint, StockMovementEndpoints>();

        return services;
    }

    public static IApplicationBuilder MapEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        foreach (var endpoint in endpoints)
            endpoint.MapEndpoints(app);

        return app;
    }
}
