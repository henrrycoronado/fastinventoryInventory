using fastinventoryInventory.Src.Application.DTOs.Units;

namespace fastinventoryInventory.Src.Application.Interfaces;

public interface IUnitService
{
    Task<UnitResponseDto?> GetByCenAsync(string unitCen);
    Task<IEnumerable<UnitResponseDto>> GetByCompanyAsync(string companyCen);
    Task<UnitResponseDto> CreateAsync(CreateUnitDto dto);
    Task<UnitResponseDto> UpdateAsync(string unitCen, UpdateUnitDto dto);
}
