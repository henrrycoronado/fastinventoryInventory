using Microsoft.EntityFrameworkCore;

using prismodInventory.Src.Domain.Entities;
using prismodInventory.Src.Infraestructure.Persistence.Interfaces;
using prismodInventory.Src.Infraestructure.Persistence.Models;

namespace prismodInventory.Src.Infraestructure.Persistence.Repositories;

public class UnitRepository : IUnitRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UnitRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<Unit?> GetByCenAsync(string unitCen)
    {
        var model = await _dbContext.Units.AsNoTracking().FirstOrDefaultAsync(u => u.UnitCen == unitCen);
        return model == null ? null : MapToDomain(model);
    }

    public async Task<IEnumerable<Unit>> GetByCompanyCenAsync(string companyCen)
    {
        var models = await _dbContext.Units.AsNoTracking().Where(u => u.CompanyCen == companyCen).ToListAsync();
        return models.Select(MapToDomain);
    }

    public async Task AddAsync(Unit unit)
    {
        var model = MapToModel(unit);
        await _dbContext.Units.AddAsync(model);
    }

    public async Task UpdateAsync(Unit unit)
    {
        var model = await _dbContext.Units.FirstOrDefaultAsync(u => u.UnitCen == unit.UnitCen);
        if (model != null)
        {
            model.Name = unit.Name;
            model.Abbreviation = unit.Abbreviation;
            model.IsActive = unit.IsActive;
            _dbContext.Units.Update(model);
        }
    }

    private static Unit MapToDomain(UnitModel model)
    {
        var unit = new Unit(model.CompanyCen, model.Name, model.Abbreviation);
        typeof(Unit).GetProperty(nameof(Unit.UnitCen))?.SetValue(unit, model.UnitCen);
        typeof(Unit).GetProperty(nameof(Unit.IsActive))?.SetValue(unit, model.IsActive);
        return unit;
    }

    private static UnitModel MapToModel(Unit entity) => new()
    {
        UnitCen = entity.UnitCen,
        CompanyCen = entity.CompanyCen,
        Name = entity.Name,
        Abbreviation = entity.Abbreviation,
        IsActive = entity.IsActive
    };
}
