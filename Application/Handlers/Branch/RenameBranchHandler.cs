using WarehouseStockService.Application.Common.Exceptions;
using WarehouseStockService.Domain.Repositories;

namespace WarehouseStockService.Application.Handlers.Branch;

public sealed record RenameBranchInput(Guid Id, string Name);

public sealed class RenameBranchHandler(IBranchRepository repo, IUnitOfWork uow)
{
    public async Task HandleAsync(RenameBranchInput input, CancellationToken ct = default)
    {
        var branch = await repo.GetByIdAsync(input.Id, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.BranchEntity), input.Id);

        branch.Rename(input.Name);

        await uow.BeginTransactionAsync(ct);
        await repo.UpdateAsync(branch, ct);
        await uow.CommitAsync(ct);
    }
}
