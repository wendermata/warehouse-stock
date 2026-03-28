using WarehouseStockService.Application.Common.Exceptions;
using WarehouseStockService.Domain.Entities;
using WarehouseStockService.Domain.Repositories;

namespace WarehouseStockService.Application.Handlers.Branch;

public sealed record CreateBranchInput(string Code, string Name);
public sealed record CreateBranchOutput(Guid Id, string Code, string Name);

public sealed class CreateBranchHandler(IBranchRepository repo, IUnitOfWork uow)
{
    public async Task<CreateBranchOutput> HandleAsync(CreateBranchInput input, CancellationToken ct = default)
    {
        var existing = await repo.GetByCodeAsync(input.Code, ct);
        if (existing is not null)
            throw new ConflictException($"Branch with code '{input.Code}' already exists.");

        var branch = BranchEntity.Create(input.Code, input.Name);

        await uow.BeginTransactionAsync(ct);
        await repo.AddAsync(branch, ct);
        await uow.CommitAsync(ct);

        return new CreateBranchOutput(branch.Id, branch.Code, branch.Name);
    }
}
