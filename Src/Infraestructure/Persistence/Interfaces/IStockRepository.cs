using prismodInventory.Src.Domain.Entities;

namespace prismodInventory.Src.Infraestructure.Persistence.Interfaces;

public interface IStockRepository
{
    Task<Stock?> GetAsync(string productCen, string warehouseCen);
    Task<IEnumerable<Stock>> GetByProductCenAsync(string productCen);
    Task<IEnumerable<Stock>> GetByWarehouseCenAsync(string warehouseCen, string? productCen = null);
    Task AddAsync(Stock stock);
    Task UpdateAsync(Stock stock);
}
