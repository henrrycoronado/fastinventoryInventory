using prismodInventory.Src.Application.DTOs.Companies;

namespace prismodInventory.Src.Application.Interfaces;

public interface ICompanyService
{
    Task<CompanyResponseDto?> GetByCenAsync(string companyCen);
    Task<CompanyLookupResponseDto?> GetLookupByCenAsync(string companyCen);
    Task<IEnumerable<CompanyResponseDto>> GetAllAsync();
    Task<CompanyResponseDto> CreateAsync(CreateCompanyDto dto);
    Task UpdateAsync(string companyCen, UpdateCompanyDto dto);
}
