using fastinventoryInventory.Src.Application.DTOs.Common;
using fastinventoryInventory.Src.Domain.Entities;
using fastinventoryInventory.Src.Infraestructure.Persistence.Interfaces;
using fastinventoryInventory.Src.Infraestructure.Persistence.Models;

using Microsoft.EntityFrameworkCore;

namespace fastinventoryInventory.Src.Infraestructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<Product?> GetByCenAsync(string productCen)
    {
        var model = await _dbContext.Products
            .Include(p => p.Category)
            .Include(p => p.Unit)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProductCen == productCen);
        return model == null ? null : MapToDomain(model);
    }

    public async Task<Product?> GetBySkuAsync(string companyCen, string sku)
    {
        var model = await _dbContext.Products
            .Include(p => p.Category)
            .Include(p => p.Unit)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.CompanyCen == companyCen && p.Sku == sku);
        return model == null ? null : MapToDomain(model);
    }

    public async Task<IEnumerable<Product>> GetByCompanyCenAsync(string companyCen, ProductQueryFilters? filters = null)
    {
        var query = _dbContext.Products
            .Include(p => p.Category)
            .Include(p => p.Unit)
            .Where(p => p.CompanyCen == companyCen);

        if (filters != null)
        {
            if (!string.IsNullOrEmpty(filters.Search))
            {
                query = query.Where(p => p.Name.Contains(filters.Search) || p.Sku.Contains(filters.Search));
            }

            if (!string.IsNullOrEmpty(filters.CategoryCen))
            {
                query = query.Where(p => p.CategoryCen == filters.CategoryCen);
            }

            if (!string.IsNullOrEmpty(filters.Status))
            {
                query = query.Where(p => p.Status == filters.Status);
            }
        }

        var models = await query.AsNoTracking().ToListAsync();
        return models.Select(MapToDomain);
    }

    public async Task<IEnumerable<Product>> GetSellableByCompanyCenAsync(string companyCen, SellableProductQueryFilters? filters = null)
    {
        var query = _dbContext.Products
            .Include(p => p.Category)
            .Include(p => p.Unit)
            .Include(p => p.Stocks)
            .Where(p => p.CompanyCen == companyCen && p.Status == "ACTIVE");

        if (filters != null)
        {
            if (!string.IsNullOrEmpty(filters.Search))
            {
                query = query.Where(p => p.Name.Contains(filters.Search) || p.Sku.Contains(filters.Search));
            }

            if (!string.IsNullOrEmpty(filters.CategoryCen))
            {
                query = query.Where(p => p.CategoryCen == filters.CategoryCen);
            }

            if (filters.OnlyAvailable)
            {
                if (!string.IsNullOrEmpty(filters.WarehouseCen))
                {
                    query = query.Where(p => p.Stocks.Any(s => s.WarehouseCen == filters.WarehouseCen && s.AvailableQuantity > 0));
                }
                else
                {
                    query = query.Where(p => p.Stocks.Any(s => s.AvailableQuantity > 0));
                }
            }

            query = query.Skip((filters.Page - 1) * filters.PageSize).Take(filters.PageSize);
        }

        var models = await query.AsNoTracking().ToListAsync();
        return models.Select(MapToDomain);
    }

    public async Task AddAsync(Product product)
    {
        var model = MapToModel(product);
        await _dbContext.Products.AddAsync(model);
    }

    public async Task UpdateAsync(Product product)
    {
        var model = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductCen == product.ProductCen);
        if (model != null)
        {
            model.Sku = product.Sku;
            model.Name = product.Name;
            model.SalePrice = product.SalePrice;
            model.CostPrice = product.CostPrice;
            model.Status = product.Status;
            model.ReorderLevel = product.ReorderLevel;
            model.CategoryCen = product.CategoryCen;
            model.UnitCen = product.UnitCen;
            model.Description = product.Description;
            model.StationCode = product.StationCode;
            _dbContext.Products.Update(model);
        }
    }

    private static Product MapToDomain(ProductModel model)
    {
        var product = new Product(model.CompanyCen, model.Sku, model.Name, model.SalePrice, model.CostPrice, model.ReorderLevel);
        typeof(Product).GetProperty(nameof(Product.ProductCen))?.SetValue(product, model.ProductCen);
        if (!string.IsNullOrEmpty(model.CategoryCen)) product.AssignCategory(model.CategoryCen);
        if (!string.IsNullOrEmpty(model.UnitCen)) product.AssignUnit(model.UnitCen);
        product.ChangeStatus(model.Status);
        typeof(Product).GetProperty(nameof(Product.Description))?.SetValue(product, model.Description);
        typeof(Product).GetProperty(nameof(Product.StationCode))?.SetValue(product, model.StationCode);
        return product;
    }

    private static ProductModel MapToModel(Product entity) => new()
    {
        ProductCen = entity.ProductCen,
        CompanyCen = entity.CompanyCen,
        CategoryCen = entity.CategoryCen,
        UnitCen = entity.UnitCen,
        Sku = entity.Sku,
        Name = entity.Name,
        Description = entity.Description,
        SalePrice = entity.SalePrice,
        CostPrice = entity.CostPrice,
        ReorderLevel = entity.ReorderLevel,
        Status = entity.Status,
        StationCode = entity.StationCode
    };
}
