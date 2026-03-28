using Microsoft.AspNetCore.Http.HttpResults;
using WarehouseStockService.Application.Common.Exceptions;
using WarehouseStockService.Application.Handlers.StockTemplate;
using WarehouseStockService.Domain.Entities;

namespace WarehouseStockService.Endpoints;

public sealed class StockTemplateEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/stock-templates").WithTags("StockTemplates");

        group.MapGet("/",          GetAll);
        group.MapGet("/{id:guid}", GetById);
        group.MapPost("/",         Create);
    }

    private static async Task<Ok<IReadOnlyList<StockTemplateEntity>>> GetAll(
        GetAllStockTemplatesHandler handler,
        CancellationToken ct)
        => TypedResults.Ok(await handler.HandleAsync(ct));

    private static async Task<Results<Ok<StockTemplateEntity>, NotFound>> GetById(
        Guid id,
        GetStockTemplateByIdHandler handler,
        CancellationToken ct)
    {
        var template = await handler.HandleAsync(id, ct);
        return template is null ? TypedResults.NotFound() : TypedResults.Ok(template);
    }

    private static async Task<Results<Created<CreateStockTemplateOutput>, Conflict>> Create(
        CreateStockTemplateRequest request,
        CreateStockTemplateHandler handler,
        CancellationToken ct)
    {
        try
        {
            var output = await handler.HandleAsync(
                new CreateStockTemplateInput(request.ExternalReference, request.Description), ct);
            return TypedResults.Created($"/stock-templates/{output.Id}", output);
        }
        catch (ConflictException)
        {
            return TypedResults.Conflict();
        }
    }

    private record CreateStockTemplateRequest(string ExternalReference, string? Description = null);
}
