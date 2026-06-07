using prismodInventory.Src.Application.DTOs.Common;
using prismodInventory.Src.Domain.Entities;

namespace prismodInventory.Src.Infraestructure.Persistence.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByCenAsync(string productCen);
    Task<Product?> GetBySkuAsync(string companyCen, string sku);
    Task<IEnumerable<Product>> GetByCompanyCenAsync(string companyCen, ProductQueryFilters? filters = null);
    Task<IEnumerable<Product>> GetSellableByCompanyCenAsync(string companyCen, SellableProductQueryFilters? filters = null);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
}
