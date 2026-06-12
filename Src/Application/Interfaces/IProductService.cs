using fastinventoryInventory.Src.Application.DTOs.Common;
using fastinventoryInventory.Src.Application.DTOs.Products;

namespace fastinventoryInventory.Src.Application.Interfaces;

public interface IProductService
{
    Task<ProductResponseDto?> GetByCenAsync(string productCen);
    Task<ProductResponseDto?> GetBySkuAsync(string companyCen, string sku);
    Task<IEnumerable<ProductResponseDto>> GetByCompanyAsync(string companyCen, ProductQueryFilters? filters = null);
    Task<IEnumerable<ProductResponseDto>> GetByCensAsync(IEnumerable<string> productCens);
    Task<CreateProductResponseDto> CreateAsync(CreateProductDto dto);
    Task<ProductResponseDto> UpdateAsync(string productCen, UpdateProductDto dto);
    Task<ProductResponseDto> UpdateStatusAsync(string productCen, UpdateProductStatusDto dto);
}
