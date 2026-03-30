using Microsoft.AspNetCore.Http.HttpResults;
using WarehouseStockService.Application.Common.Exceptions;
using WarehouseStockService.Application.Handlers.ItemLocation;
using WarehouseStockService.Domain.ReadModels;

namespace WarehouseStockService.Endpoints;

public sealed class ItemLocationEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/item-locations").WithTags("ItemLocations");

        group.MapGet("/{id:guid}",        GetById);
        group.MapGet("/by-location",      GetByLocation);
        group.MapGet("/by-sku",           GetBySku);
        group.MapPost("/",                Create);
        group.MapPost("/{id:guid}/entry", ApplyEntry);
        group.MapPost("/{id:guid}/exit",  ApplyExit);
    }

    private static async Task<Results<Ok<ItemLocationDetail>, NotFound>> GetById(
        Guid id,
        GetItemLocationByIdHandler handler,
        CancellationToken ct)
    {
        var item = await handler.HandleAsync(id, ct);
        return item is null ? TypedResults.NotFound() : TypedResults.Ok(item);
    }

    private static async Task<Results<Ok<IReadOnlyList<ItemLocationDetail>>, BadRequest<string>>> GetByLocation(
        Guid? stockLocationId,
        GetItemLocationsByLocationHandler handler,
        CancellationToken ct)
    {
        if (stockLocationId is null)
            return TypedResults.BadRequest("Query parameter 'stockLocationId' is required.");

        return TypedResults.Ok(await handler.HandleAsync(stockLocationId.Value, ct));
    }

    private static async Task<Results<Ok<IReadOnlyList<ItemLocationDetail>>, BadRequest<string>>> GetBySku(
        string? sku,
        GetItemLocationsBySkuHandler handler,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(sku))
            return TypedResults.BadRequest("Query parameter 'sku' is required.");

        return TypedResults.Ok(await handler.HandleAsync(sku, ct));
    }

    private static async Task<Results<Created<CreateItemLocationOutput>, NotFound, Conflict>> Create(
        CreateItemLocationRequest request,
        CreateItemLocationHandler handler,
        CancellationToken ct)
    {
        try
        {
            var output = await handler.HandleAsync(
                new CreateItemLocationInput(request.StockLocationId, request.Sku, request.InitialQuantity), ct);
            return TypedResults.Created($"/item-locations/{output.Id}", output);
        }
        catch (NotFoundException)
        {
            return TypedResults.NotFound();
        }
        catch (ConflictException)
        {
            return TypedResults.Conflict();
        }
    }

    private static async Task<Results<Ok<ItemLocationOutput>, NotFound>> ApplyEntry(
        Guid id,
        MovementRequest request,
        ApplyEntryHandler handler,
        CancellationToken ct)
    {
        try
        {
            var output = await handler.HandleAsync(new ApplyEntryInput(id, request.Quantity, request.ExternalReference), ct);
            return TypedResults.Ok(output);
        }
        catch (NotFoundException)
        {
            return TypedResults.NotFound();
        }
    }

    private static async Task<Results<Ok<ItemLocationOutput>, NotFound, UnprocessableEntity<string>>> ApplyExit(
        Guid id,
        MovementRequest request,
        ApplyExitHandler handler,
        CancellationToken ct)
    {
        try
        {
            var output = await handler.HandleAsync(new ApplyExitInput(id, request.Quantity, request.ExternalReference), ct);
            return TypedResults.Ok(output);
        }
        catch (NotFoundException)
        {
            return TypedResults.NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return TypedResults.UnprocessableEntity(ex.Message);
        }
    }

    private record CreateItemLocationRequest(Guid StockLocationId, string Sku, int InitialQuantity = 0);
    private record MovementRequest(int Quantity, string ExternalReference);
}
