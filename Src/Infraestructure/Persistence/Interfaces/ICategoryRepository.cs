using fastinventoryInventory.Src.Domain.Entities;

namespace fastinventoryInventory.Src.Infraestructure.Persistence.Interfaces;

public interface ICategoryRepository
{
    Task<Category?> GetByCenAsync(string categoryCen);
    Task<IEnumerable<Category>> GetByCompanyCenAsync(string companyCen);
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
}
