using prismodInventory.Src.Application.DTOs.Common;
using prismodInventory.Src.Application.DTOs.Products;

namespace prismodInventory.Src.Application.Interfaces;

public interface IProductService
{
    Task<ProductResponseDto?> GetByCenAsync(string productCen);
    Task<ProductResponseDto?> GetBySkuAsync(string companyCen, string sku);
    Task<IEnumerable<ProductResponseDto>> GetByCompanyAsync(string companyCen, ProductQueryFilters? filters = null);
    Task<IEnumerable<ProductResponseDto>> GetByCensAsync(IEnumerable<string> productCens);
    Task<CreateProductResponseDto> CreateAsync(CreateProductDto dto);
    Task UpdateAsync(string productCen, UpdateProductDto dto);
    Task UpdateStatusAsync(string productCen, UpdateProductStatusDto dto);
}
