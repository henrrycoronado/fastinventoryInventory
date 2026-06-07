using fastinventoryInventory.Src.Application.DTOs.Companies;

namespace fastinventoryInventory.Src.Application.Interfaces;

public interface ICompanyService
{
    Task<CompanyResponseDto?> GetByCenAsync(string companyCen);
    Task<CompanyLookupResponseDto?> GetLookupByCenAsync(string companyCen);
    Task<IEnumerable<CompanyResponseDto>> GetAllAsync();
    Task<CompanyResponseDto> CreateAsync(CreateCompanyDto dto);
    Task UpdateAsync(string companyCen, UpdateCompanyDto dto);
}
