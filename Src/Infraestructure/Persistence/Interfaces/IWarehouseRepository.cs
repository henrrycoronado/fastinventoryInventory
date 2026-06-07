using fastinventoryInventory.Src.Domain.Entities;

namespace fastinventoryInventory.Src.Infraestructure.Persistence.Interfaces;

public interface IWarehouseRepository
{
    Task<Warehouse?> GetByCenAsync(string warehouseCen);
    Task<IEnumerable<Warehouse>> GetByCompanyCenAsync(string companyCen);
    Task AddAsync(Warehouse warehouse);
    Task UpdateAsync(Warehouse warehouse);
}
