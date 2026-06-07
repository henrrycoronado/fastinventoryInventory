using fastinventoryInventory.Src.Application.DTOs.Common;
using fastinventoryInventory.Src.Domain.Entities;

namespace fastinventoryInventory.Src.Infraestructure.Persistence.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByCenAsync(string productCen);
    Task<Product?> GetBySkuAsync(string companyCen, string sku);
    Task<IEnumerable<Product>> GetByCompanyCenAsync(string companyCen, ProductQueryFilters? filters = null);
    Task<IEnumerable<Product>> GetSellableByCompanyCenAsync(string companyCen, SellableProductQueryFilters? filters = null);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
}
