using Microsoft.EntityFrameworkCore;

using prismodInventory.Src.Domain.Entities;
using prismodInventory.Src.Infraestructure.Persistence.Interfaces;
using prismodInventory.Src.Infraestructure.Persistence.Models;

namespace prismodInventory.Src.Infraestructure.Persistence.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly ApplicationDbContext _dbContext;

    public WarehouseRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<Warehouse?> GetByCenAsync(string warehouseCen)
    {
        var model = await _dbContext.Warehouses.AsNoTracking().FirstOrDefaultAsync(w => w.WarehouseCen == warehouseCen);
        return model == null ? null : MapToDomain(model);
    }

    public async Task<IEnumerable<Warehouse>> GetByCompanyCenAsync(string companyCen)
    {
        var models = await _dbContext.Warehouses.AsNoTracking().Where(w => w.CompanyCen == companyCen).ToListAsync();
        return models.Select(MapToDomain);
    }

    public async Task AddAsync(Warehouse warehouse)
    {
        var model = MapToModel(warehouse);
        await _dbContext.Warehouses.AddAsync(model);
    }

    public async Task UpdateAsync(Warehouse warehouse)
    {
        var model = await _dbContext.Warehouses.FirstOrDefaultAsync(w => w.WarehouseCen == warehouse.WarehouseCen);
        if (model != null)
        {
            model.Name = warehouse.Name;
            model.IsActive = warehouse.IsActive;
            _dbContext.Warehouses.Update(model);
        }
    }

    private static Warehouse MapToDomain(WarehouseModel model)
    {
        var warehouse = new Warehouse(model.CompanyCen, model.Name);
        typeof(Warehouse).GetProperty(nameof(Warehouse.WarehouseCen))?.SetValue(warehouse, model.WarehouseCen);
        if (!model.IsActive) warehouse.Deactivate();
        return warehouse;
    }

    private static WarehouseModel MapToModel(Warehouse entity) => new()
    {
        WarehouseCen = entity.WarehouseCen,
        CompanyCen = entity.CompanyCen,
        Name = entity.Name,
        IsActive = entity.IsActive
    };
}
