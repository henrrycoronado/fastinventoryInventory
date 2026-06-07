using prismodInventory.Src.Application.DTOs.Categories;
using prismodInventory.Src.Application.Interfaces;
using prismodInventory.Src.Domain.Entities;
using prismodInventory.Src.Infraestructure.Persistence.Interfaces;

namespace prismodInventory.Src.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(ICategoryRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoryResponseDto?> GetByCenAsync(string categoryCen)
    {
        var entity = await _repository.GetByCenAsync(categoryCen);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetByCompanyAsync(string companyCen)
    {
        var entities = await _repository.GetByCompanyCenAsync(companyCen);
        return entities.Select(MapToDto);
    }

    public async Task<CategoryResponseDto> CreateAsync(CreateCategoryDto dto)
    {
        var entity = new Category(dto.CompanyCen, dto.Name, dto.Description);
        await _repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task UpdateAsync(string categoryCen, UpdateCategoryDto dto)
    {
        var entity = await _repository.GetByCenAsync(categoryCen);
        if (entity == null)
        {
            throw new InvalidOperationException("Category not found");
        }

        typeof(Category).GetProperty(nameof(Category.CategoryCen))?.SetValue(entity, categoryCen);
        typeof(Category).GetProperty(nameof(Category.Name))?.SetValue(entity, dto.Name);
        typeof(Category).GetProperty(nameof(Category.Description))?.SetValue(entity, dto.Description);
        typeof(Category).GetProperty(nameof(Category.IsActive))?.SetValue(entity, dto.IsActive);

        await _repository.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    private static CategoryResponseDto MapToDto(Category entity) => new()
    {
        CategoryCen = entity.CategoryCen,
        CompanyCen = entity.CompanyCen,
        Name = entity.Name,
        Description = entity.Description,
        IsActive = entity.IsActive
    };
}
