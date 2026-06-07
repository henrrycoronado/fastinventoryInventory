using prismodInventory.Src.Application.DTOs.Warehouses;
using prismodInventory.Src.Application.Interfaces;
using prismodInventory.Src.Domain.Entities;
using prismodInventory.Src.Infraestructure.Persistence.Interfaces;

namespace prismodInventory.Src.Application.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public WarehouseService(IWarehouseRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<WarehouseResponseDto?> GetByCenAsync(string warehouseCen)
    {
        var entity = await _repository.GetByCenAsync(warehouseCen);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<IEnumerable<WarehouseResponseDto>> GetByCompanyAsync(string companyCen)
    {
        var entities = await _repository.GetByCompanyCenAsync(companyCen);
        return entities.Select(MapToDto);
    }

    public async Task<WarehouseResponseDto> CreateAsync(CreateWarehouseDto dto)
    {
        var entity = new Warehouse(dto.CompanyCen, dto.Name);
        await _repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task UpdateAsync(string warehouseCen, UpdateWarehouseDto dto)
    {
        var entity = await _repository.GetByCenAsync(warehouseCen);
        if (entity == null)
        {
            throw new InvalidOperationException("Warehouse not found");
        }

        typeof(Warehouse).GetProperty(nameof(Warehouse.WarehouseCen))?.SetValue(entity, warehouseCen);
        typeof(Warehouse).GetProperty(nameof(Warehouse.Name))?.SetValue(entity, dto.Name);
        if (dto.IsActive)
        {
            typeof(Warehouse).GetProperty(nameof(Warehouse.IsActive))?.SetValue(entity, true);
        }
        else
        {
            entity.Deactivate();
        }

        await _repository.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    private static WarehouseResponseDto MapToDto(Warehouse entity) => new()
    {
        WarehouseCen = entity.WarehouseCen,
        CompanyCen = entity.CompanyCen,
        Name = entity.Name,
        IsActive = entity.IsActive
    };
}
