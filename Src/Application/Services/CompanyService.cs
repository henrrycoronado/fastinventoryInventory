using fastinventoryInventory.Src.Application.DTOs.Companies;
using fastinventoryInventory.Src.Application.Interfaces;
using fastinventoryInventory.Src.Domain.Entities;
using fastinventoryInventory.Src.Infraestructure.Persistence.Interfaces;

namespace fastinventoryInventory.Src.Application.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CompanyService(ICompanyRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CompanyResponseDto?> GetByCenAsync(string companyCen)
    {
        var company = await _repository.GetByCenAsync(companyCen);
        return company == null ? null : MapToDto(company);
    }

    public async Task<CompanyLookupResponseDto?> GetLookupByCenAsync(string companyCen)
    {
        var result = await _repository.GetWithInternalIdByCenAsync(companyCen);
        if (result.Entity == null) return null;

        return new CompanyLookupResponseDto
        {
            CompanyCen = result.Entity.CompanyCen,
            Name = result.Entity.Name,
            CompanyId = (int)result.Id
        };
    }

    public async Task<IEnumerable<CompanyResponseDto>> GetAllAsync()
    {
        var companies = await _repository.GetAllAsync();
        return companies.Select(MapToDto);
    }

    public async Task<CompanyResponseDto> CreateAsync(CreateCompanyDto dto)
    {
        var company = new Company(dto.Name);
        await _repository.AddAsync(company);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(company);
    }

    public async Task UpdateAsync(string companyCen, UpdateCompanyDto dto)
    {
        var company = await _repository.GetByCenAsync(companyCen);
        if (company == null)
        {
            throw new InvalidOperationException("Company not found");
        }

        typeof(Company).GetProperty(nameof(Company.CompanyCen))?.SetValue(company, companyCen);
        typeof(Company).GetProperty(nameof(Company.Name))?.SetValue(company, dto.Name);
        if (dto.IsActive)
        {
            company.Activate();
        }
        else
        {
            company.Deactivate();
        }

        await _repository.UpdateAsync(company);
        await _unitOfWork.SaveChangesAsync();
    }

    private static CompanyResponseDto MapToDto(Company entity) => new()
    {
        CompanyCen = entity.CompanyCen,
        Name = entity.Name,
        IsActive = entity.IsActive,
        CreatedAt = entity.CreatedAt
    };
}
