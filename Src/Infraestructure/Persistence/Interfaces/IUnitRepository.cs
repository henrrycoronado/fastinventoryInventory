using fastinventoryInventory.Src.Domain.Entities;

namespace fastinventoryInventory.Src.Infraestructure.Persistence.Interfaces;

public interface IUnitRepository
{
    Task<Unit?> GetByCenAsync(string unitCen);
    Task<IEnumerable<Unit>> GetByCompanyCenAsync(string companyCen);
    Task AddAsync(Unit unit);
    Task UpdateAsync(Unit unit);
}
