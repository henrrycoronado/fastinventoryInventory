using Microsoft.EntityFrameworkCore;

using prismodInventory.Src.Domain.Entities;
using prismodInventory.Src.Infraestructure.Persistence.Interfaces;
using prismodInventory.Src.Infraestructure.Persistence.Models;

namespace prismodInventory.Src.Infraestructure.Persistence.Repositories;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDbContext _dbContext;

    public StockRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<Stock?> GetAsync(string productCen, string warehouseCen)
    {
        var model = await _dbContext.Stocks.AsNoTracking()
            .FirstOrDefaultAsync(s => s.ProductCen == productCen && s.WarehouseCen == warehouseCen);

        return model == null ? null : MapToDomain(model);
    }

    public async Task<IEnumerable<Stock>> GetByProductCenAsync(string productCen)
    {
        var models = await _dbContext.Stocks.AsNoTracking().Where(s => s.ProductCen == productCen).ToListAsync();
        return models.Select(MapToDomain);
    }

    public async Task<IEnumerable<Stock>> GetByWarehouseCenAsync(string warehouseCen, string? productCen = null)
    {
        var query = _dbContext.Stocks.AsNoTracking().Where(s => s.WarehouseCen == warehouseCen);

        if (!string.IsNullOrEmpty(productCen))
        {
            query = query.Where(s => s.ProductCen == productCen);
        }

        var models = await query.ToListAsync();
        return models.Select(MapToDomain);
    }

    public async Task AddAsync(Stock stock)
    {
        var model = MapToModel(stock);
        await _dbContext.Stocks.AddAsync(model);
    }

    public async Task UpdateAsync(Stock stock)
    {
        var model = await _dbContext.Stocks.FirstOrDefaultAsync(s => s.StockCen == stock.StockCen);
        if (model != null)
        {
            model.AvailableQuantity = stock.AvailableQuantity;
            model.ReservedQuantity = stock.ReservedQuantity;
            _dbContext.Stocks.Update(model);
        }
    }

    private static Stock MapToDomain(StockModel model)
    {
        var stock = new Stock(model.ProductCen, model.WarehouseCen, model.AvailableQuantity);
        typeof(Stock).GetProperty(nameof(Stock.StockCen))?.SetValue(stock, model.StockCen);
        typeof(Stock).GetProperty(nameof(Stock.ReservedQuantity))?.SetValue(stock, model.ReservedQuantity);
        return stock;
    }

    private static StockModel MapToModel(Stock entity) => new()
    {
        StockCen = entity.StockCen,
        ProductCen = entity.ProductCen,
        WarehouseCen = entity.WarehouseCen,
        AvailableQuantity = entity.AvailableQuantity,
        ReservedQuantity = entity.ReservedQuantity
    };
}
