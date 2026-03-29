using Microsoft.AspNetCore.Http.HttpResults;
using WarehouseStockService.Application.Common.Exceptions;
using WarehouseStockService.Application.Handlers.StockLocation;
using WarehouseStockService.Domain.ReadModels;

namespace WarehouseStockService.Endpoints;

public sealed class StockLocationEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/stock-locations").WithTags("StockLocations");

        group.MapGet("/",          GetByBranch);
        group.MapGet("/{id:guid}", GetById);
        group.MapPost("/",         Create);
    }

    private static async Task<Results<Ok<IReadOnlyList<StockLocationDetail>>, BadRequest<string>>> GetByBranch(
        Guid? branchId,
        GetStockLocationsByBranchHandler handler,
        CancellationToken ct)
    {
        if (branchId is null)
            return TypedResults.BadRequest("Query parameter 'branchId' is required.");

        return TypedResults.Ok(await handler.HandleAsync(branchId.Value, ct));
    }

    private static async Task<Results<Ok<StockLocationDetail>, NotFound>> GetById(
        Guid id,
        GetStockLocationByIdHandler handler,
        CancellationToken ct)
    {
        var location = await handler.HandleAsync(id, ct);
        return location is null ? TypedResults.NotFound() : TypedResults.Ok(location);
    }

    private static async Task<Results<Created<CreateStockLocationOutput>, NotFound, Conflict>> Create(
        CreateStockLocationRequest request,
        CreateStockLocationHandler handler,
        CancellationToken ct)
    {
        try
        {
            var output = await handler.HandleAsync(new CreateStockLocationInput(request.BranchId, request.StockTemplateId), ct);
            return TypedResults.Created($"/stock-locations/{output.Id}", output);
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

    private record CreateStockLocationRequest(Guid BranchId, Guid StockTemplateId);
}
