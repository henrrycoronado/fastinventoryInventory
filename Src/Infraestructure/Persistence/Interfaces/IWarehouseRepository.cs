using prismodInventory.Src.Domain.Entities;

namespace prismodInventory.Src.Infraestructure.Persistence.Interfaces;

public interface IWarehouseRepository
{
    Task<Warehouse?> GetByCenAsync(string warehouseCen);
    Task<IEnumerable<Warehouse>> GetByCompanyCenAsync(string companyCen);
    Task AddAsync(Warehouse warehouse);
    Task UpdateAsync(Warehouse warehouse);
}
