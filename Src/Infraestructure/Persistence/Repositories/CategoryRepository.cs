using fastinventoryInventory.Src.Domain.Entities;
using fastinventoryInventory.Src.Infraestructure.Persistence.Interfaces;
using fastinventoryInventory.Src.Infraestructure.Persistence.Models;

using Microsoft.EntityFrameworkCore;

namespace fastinventoryInventory.Src.Infraestructure.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CategoryRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<Category?> GetByCenAsync(string categoryCen)
    {
        var model = await _dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.CategoryCen == categoryCen);
        return model == null ? null : MapToDomain(model);
    }

    public async Task<IEnumerable<Category>> GetByCompanyCenAsync(string companyCen)
    {
        var models = await _dbContext.Categories.AsNoTracking().Where(c => c.CompanyCen == companyCen).ToListAsync();
        return models.Select(MapToDomain);
    }

    public async Task AddAsync(Category category)
    {
        var model = MapToModel(category);
        await _dbContext.Categories.AddAsync(model);
    }

    public async Task UpdateAsync(Category category)
    {
        var model = await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryCen == category.CategoryCen);
        if (model != null)
        {
            model.Name = category.Name;
            model.Description = category.Description;
            model.IsActive = category.IsActive;
            _dbContext.Categories.Update(model);
        }
    }

    private static Category MapToDomain(CategoryModel model)
    {
        var category = new Category(model.CompanyCen, model.Name, model.Description);
        typeof(Category).GetProperty(nameof(Category.CategoryCen))?.SetValue(category, model.CategoryCen);
        typeof(Category).GetProperty(nameof(Category.IsActive))?.SetValue(category, model.IsActive);
        return category;
    }

    private static CategoryModel MapToModel(Category entity) => new()
    {
        CategoryCen = entity.CategoryCen,
        CompanyCen = entity.CompanyCen,
        Name = entity.Name,
        Description = entity.Description,
        IsActive = entity.IsActive
    };
}
