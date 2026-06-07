using prismodInventory.Src.Domain.Entities;

namespace prismodInventory.Src.Infraestructure.Persistence.Interfaces;

public interface ICompanyRepository
{
    Task<Company?> GetByCenAsync(string companyCen);
    Task<(Company? Entity, long Id)> GetWithInternalIdByCenAsync(string companyCen);
    Task<IEnumerable<Company>> GetAllAsync();
    Task AddAsync(Company company);
    Task UpdateAsync(Company company);
}
