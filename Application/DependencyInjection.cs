using Microsoft.Extensions.DependencyInjection;
using WarehouseStockService.Application.Handlers.Branch;
using WarehouseStockService.Application.Handlers.ItemLocation;
using WarehouseStockService.Application.Handlers.StockLocation;
using WarehouseStockService.Application.Handlers.StockTemplate;
using WarehouseStockService.Application.Handlers.StockMovement;

namespace WarehouseStockService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Branch
        services.AddScoped<CreateBranchHandler>();
        services.AddScoped<RenameBranchHandler>();
        services.AddScoped<GetAllBranchesHandler>();
        services.AddScoped<GetBranchByIdHandler>();

        // StockTemplate
        services.AddScoped<CreateStockTemplateHandler>();
        services.AddScoped<GetAllStockTemplatesHandler>();
        services.AddScoped<GetStockTemplateByIdHandler>();

        // StockLocation
        services.AddScoped<CreateStockLocationHandler>();
        services.AddScoped<GetStockLocationByIdHandler>();
        services.AddScoped<GetStockLocationsByBranchHandler>();

        // ItemLocation
        services.AddScoped<CreateItemLocationHandler>();
        services.AddScoped<ApplyEntryHandler>();
        services.AddScoped<ApplyExitHandler>();
        services.AddScoped<GetItemLocationByIdHandler>();
        services.AddScoped<GetItemLocationsByLocationHandler>();
        services.AddScoped<GetItemLocationsBySkuHandler>();

        // StockMovement
        services.AddScoped<GetStockMovementByIdHandler>();
        services.AddScoped<GetStockMovementsByItemLocationHandler>();

        return services;
    }
}
