using Microsoft.AspNetCore.Http.HttpResults;
using WarehouseStockService.Application.Common.Exceptions;
using WarehouseStockService.Application.Handlers.Branch;
using WarehouseStockService.Domain.Entities;

namespace WarehouseStockService.Endpoints;

public sealed class BranchEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/branches").WithTags("Branches");

        group.MapGet("/",          GetAll);
        group.MapGet("/{id:guid}", GetById);
        group.MapPost("/",         Create);
        group.MapPut("/{id:guid}", Update);
    }

    private static async Task<Ok<IReadOnlyList<BranchEntity>>> GetAll(
        GetAllBranchesHandler handler,
        CancellationToken ct)
        => TypedResults.Ok(await handler.HandleAsync(ct));

    private static async Task<Results<Ok<BranchEntity>, NotFound>> GetById(
        Guid id,
        GetBranchByIdHandler handler,
        CancellationToken ct)
    {
        var branch = await handler.HandleAsync(id, ct);
        return branch is null ? TypedResults.NotFound() : TypedResults.Ok(branch);
    }

    private static async Task<Results<Created<CreateBranchOutput>, Conflict>> Create(
        CreateBranchRequest request,
        CreateBranchHandler handler,
        CancellationToken ct)
    {
        try
        {
            var output = await handler.HandleAsync(new CreateBranchInput(request.Code, request.Name), ct);
            return TypedResults.Created($"/branches/{output.Id}", output);
        }
        catch (ConflictException)
        {
            return TypedResults.Conflict();
        }
    }

    private static async Task<Results<NoContent, NotFound>> Update(
        Guid id,
        UpdateBranchRequest request,
        RenameBranchHandler handler,
        CancellationToken ct)
    {
        try
        {
            await handler.HandleAsync(new RenameBranchInput(id, request.Name), ct);
            return TypedResults.NoContent();
        }
        catch (NotFoundException)
        {
            return TypedResults.NotFound();
        }
    }

    private record CreateBranchRequest(string Code, string Name);
    private record UpdateBranchRequest(string Name);
}
