using fastinventoryInventory.Src.Application.DTOs.Warehouses;

namespace fastinventoryInventory.Src.Application.Interfaces;

public interface IWarehouseService
{
    Task<WarehouseResponseDto?> GetByCenAsync(string warehouseCen);
    Task<IEnumerable<WarehouseResponseDto>> GetByCompanyAsync(string companyCen);
    Task<WarehouseResponseDto> CreateAsync(CreateWarehouseDto dto);
    Task UpdateAsync(string warehouseCen, UpdateWarehouseDto dto);
}
