using fastinventoryInventory.Src.Application.DTOs.Common;
using fastinventoryInventory.Src.Domain.Entities;
using fastinventoryInventory.Src.Infraestructure.Persistence.Interfaces;
using fastinventoryInventory.Src.Infraestructure.Persistence.Models;

using Microsoft.EntityFrameworkCore;

namespace fastinventoryInventory.Src.Infraestructure.Persistence.Repositories;

public class KardexMovementRepository : IKardexMovementRepository
{
    private readonly ApplicationDbContext _dbContext;

    public KardexMovementRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<IEnumerable<KardexMovement>> GetByProductAndWarehouseAsync(string productCen, string warehouseCen, KardexQueryFilters? filters = null)
    {
        var query = _dbContext.KardexMovements.AsNoTracking().Where(k => k.ProductCen == productCen && k.WarehouseCen == warehouseCen);

        if (filters != null)
        {
            if (filters.From.HasValue)
            {
                query = query.Where(k => k.CreatedAt >= filters.From.Value);
            }

            if (filters.To.HasValue)
            {
                query = query.Where(k => k.CreatedAt <= filters.To.Value);
            }
        }

        var models = await query.ToListAsync();
        return models.Select(MapToDomain);
    }

    public async Task<IEnumerable<KardexMovement>> GetByDocumentCenAsync(string documentCen)
    {
        var models = await _dbContext.KardexMovements.AsNoTracking().Where(k => k.DocumentCen == documentCen).ToListAsync();
        return models.Select(MapToDomain);
    }

    public async Task AddAsync(KardexMovement movement)
    {
        var model = MapToModel(movement);
        await _dbContext.KardexMovements.AddAsync(model);
    }

    private static KardexMovement MapToDomain(KardexMovementModel model)
    {
        var movement = new KardexMovement(model.CompanyCen, model.WarehouseCen, model.ProductCen, model.MovementType, model.Quantity, model.DocumentCen, model.UnitCost, model.Reason);
        typeof(KardexMovement).GetProperty(nameof(KardexMovement.MovementCen))?.SetValue(movement, model.MovementCen);
        typeof(KardexMovement).GetProperty(nameof(KardexMovement.CreatedAt))?.SetValue(movement, model.CreatedAt);
        return movement;
    }

    private static KardexMovementModel MapToModel(KardexMovement entity) => new()
    {
        MovementCen = entity.MovementCen,
        CompanyCen = entity.CompanyCen,
        WarehouseCen = entity.WarehouseCen,
        ProductCen = entity.ProductCen,
        DocumentCen = entity.DocumentCen,
        MovementType = entity.MovementType,
        Quantity = entity.Quantity,
        UnitCost = entity.UnitCost,
        Reason = entity.Reason,
        CreatedAt = entity.CreatedAt
    };
}
