using WarehouseStockService.Application.Common.Exceptions;
using WarehouseStockService.Domain.Entities;
using WarehouseStockService.Domain.Repositories;

namespace WarehouseStockService.Application.Handlers.StockLocation;

public sealed record CreateStockLocationInput(Guid BranchId, Guid StockTemplateId);
public sealed record CreateStockLocationOutput(Guid Id, Guid BranchId, Guid StockTemplateId);

public sealed class CreateStockLocationHandler(
    IStockLocationRepository repo,
    IBranchRepository branchRepo,
    IStockTemplateRepository templateRepo,
    IUnitOfWork uow)
{
    public async Task<CreateStockLocationOutput> HandleAsync(CreateStockLocationInput input, CancellationToken ct = default)
    {
        _ = await branchRepo.GetByIdAsync(input.BranchId, ct)
            ?? throw new NotFoundException(nameof(BranchEntity), input.BranchId);

        _ = await templateRepo.GetByIdAsync(input.StockTemplateId, ct)
            ?? throw new NotFoundException(nameof(StockTemplateEntity), input.StockTemplateId);

        var existing = await repo.GetByTemplateAndBranchAsync(input.BranchId, input.StockTemplateId, ct);
        if (existing is not null)
            throw new ConflictException($"Stock location with template '{input.StockTemplateId}' already exists in branch '{input.BranchId}'.");

        var location = StockLocationEntity.Create(input.BranchId, input.StockTemplateId);

        await uow.BeginTransactionAsync(ct);
        await repo.AddAsync(location, ct);
        await uow.CommitAsync(ct);

        return new CreateStockLocationOutput(location.Id, location.BranchId, location.StockTemplateId);
    }
}
