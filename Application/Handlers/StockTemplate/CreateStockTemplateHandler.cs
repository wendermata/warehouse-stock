using WarehouseStockService.Application.Common.Exceptions;
using WarehouseStockService.Domain.Entities;
using WarehouseStockService.Domain.Repositories;

namespace WarehouseStockService.Application.Handlers.StockTemplate;

public sealed record CreateStockTemplateInput(string ExternalReference, string? Description = null);
public sealed record CreateStockTemplateOutput(Guid Id, string ExternalReference, string? Description);

public sealed class CreateStockTemplateHandler(
    IStockTemplateRepository repo,
    IUnitOfWork uow)
{
    public async Task<CreateStockTemplateOutput> HandleAsync(CreateStockTemplateInput input, CancellationToken ct = default)
    {
        var existing = await repo.GetByExternalReferenceAsync(input.ExternalReference, ct);
        if (existing is not null)
            throw new ConflictException($"Stock template '{input.ExternalReference}' already exists.");

        var template = StockTemplateEntity.Create(input.ExternalReference, input.Description);

        await uow.BeginTransactionAsync(ct);
        await repo.AddAsync(template, ct);
        await uow.CommitAsync(ct);

        return new CreateStockTemplateOutput(template.Id, template.ExternalReference, template.Description);
    }
}
