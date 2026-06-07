using prismodInventory.Src.Application.DTOs.Categories;

namespace prismodInventory.Src.Application.Interfaces;

public interface ICategoryService
{
    Task<CategoryResponseDto?> GetByCenAsync(string categoryCen);
    Task<IEnumerable<CategoryResponseDto>> GetByCompanyAsync(string companyCen);
    Task<CategoryResponseDto> CreateAsync(CreateCategoryDto dto);
    Task UpdateAsync(string categoryCen, UpdateCategoryDto dto);
}
