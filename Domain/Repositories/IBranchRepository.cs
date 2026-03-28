using WarehouseStockService.Domain.Entities;

namespace WarehouseStockService.Domain.Repositories;

public interface IBranchRepository
{
    Task<BranchEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BranchEntity?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<IReadOnlyList<BranchEntity>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(BranchEntity branch, CancellationToken ct = default);
    Task UpdateAsync(BranchEntity branch, CancellationToken ct = default);
}
