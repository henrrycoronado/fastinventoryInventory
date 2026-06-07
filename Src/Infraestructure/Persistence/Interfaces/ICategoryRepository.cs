using prismodInventory.Src.Domain.Entities;

namespace prismodInventory.Src.Infraestructure.Persistence.Interfaces;

public interface ICategoryRepository
{
    Task<Category?> GetByCenAsync(string categoryCen);
    Task<IEnumerable<Category>> GetByCompanyCenAsync(string companyCen);
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
}
