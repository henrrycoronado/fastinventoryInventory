using fastinventoryInventory.Src.Domain.Entities;
using fastinventoryInventory.Src.Infraestructure.Persistence.Interfaces;
using fastinventoryInventory.Src.Infraestructure.Persistence.Models;

using Microsoft.EntityFrameworkCore;

namespace fastinventoryInventory.Src.Infraestructure.Persistence.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CompanyRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<Company?> GetByCenAsync(string companyCen)
    {
        var model = await _dbContext.Companies.AsNoTracking().FirstOrDefaultAsync(c => c.CompanyCen == companyCen);
        return model == null ? null : MapToDomain(model);
    }

    public async Task<(Company? Entity, long Id)> GetWithInternalIdByCenAsync(string companyCen)
    {
        var model = await _dbContext.Companies.AsNoTracking().FirstOrDefaultAsync(c => c.CompanyCen == companyCen);
        return model == null ? (null, 0) : (MapToDomain(model), model.Id);
    }

    public async Task<IEnumerable<Company>> GetAllAsync()
    {
        var models = await _dbContext.Companies.AsNoTracking().ToListAsync();
        return models.Select(MapToDomain);
    }

    public async Task AddAsync(Company company)
    {
        var model = MapToModel(company);
        await _dbContext.Companies.AddAsync(model);
    }

    public async Task UpdateAsync(Company company)
    {
        var model = await _dbContext.Companies.FirstOrDefaultAsync(c => c.CompanyCen == company.CompanyCen);
        if (model != null)
        {
            model.Name = company.Name;
            model.IsActive = company.IsActive;
            _dbContext.Companies.Update(model);
        }
    }

    private static Company MapToDomain(CompanyModel model)
    {
        var company = new Company(model.Name);
        typeof(Company).GetProperty(nameof(Company.CompanyCen))?.SetValue(company, model.CompanyCen);
        typeof(Company).GetProperty(nameof(Company.CreatedAt))?.SetValue(company, model.CreatedAt);
        if (!model.IsActive) company.Deactivate();
        return company;
    }

    private static CompanyModel MapToModel(Company entity) => new()
    {
        CompanyCen = entity.CompanyCen,
        Name = entity.Name,
        IsActive = entity.IsActive,
        CreatedAt = entity.CreatedAt
    };
}
