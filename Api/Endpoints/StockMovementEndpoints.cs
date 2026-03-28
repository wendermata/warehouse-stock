using Microsoft.AspNetCore.Http.HttpResults;
using WarehouseStockService.Application.Handlers.StockMovement;
using WarehouseStockService.Domain.Entities;

namespace WarehouseStockService.Endpoints;

/// <summary>
/// Read-only endpoints for the stock movement ledger.
/// Movements are created exclusively through ItemLocation entry/exit operations.
/// </summary>
public sealed class StockMovementEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/stock-movements").WithTags("StockMovements");

        group.MapGet("/{id:guid}", GetById);
        group.MapGet("/by-item",   GetByItemLocation);
    }

    private static async Task<Results<Ok<StockMovementEntity>, NotFound>> GetById(
        Guid id,
        GetStockMovementByIdHandler handler,
        CancellationToken ct)
    {
        var movement = await handler.HandleAsync(id, ct);
        return movement is null ? TypedResults.NotFound() : TypedResults.Ok(movement);
    }

    private static async Task<Results<Ok<IReadOnlyList<StockMovementEntity>>, BadRequest<string>>> GetByItemLocation(
        Guid? itemLocationId,
        GetStockMovementsByItemLocationHandler handler,
        int limit = 50,
        int offset = 0,
        CancellationToken ct = default)
    {
        if (itemLocationId is null)
            return TypedResults.BadRequest("Query parameter 'itemLocationId' is required.");

        var movements = await handler.HandleAsync(
            new GetStockMovementsByItemLocationInput(itemLocationId.Value, limit, offset), ct);
        return TypedResults.Ok(movements);
    }
}
