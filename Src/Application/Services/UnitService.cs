using prismodInventory.Src.Application.DTOs.Units;
using prismodInventory.Src.Application.Interfaces;
using prismodInventory.Src.Domain.Entities;
using prismodInventory.Src.Infraestructure.Persistence.Interfaces;

namespace prismodInventory.Src.Application.Services;

public class UnitService : IUnitService
{
    private readonly IUnitRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UnitService(IUnitRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UnitResponseDto?> GetByCenAsync(string unitCen)
    {
        var entity = await _repository.GetByCenAsync(unitCen);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<IEnumerable<UnitResponseDto>> GetByCompanyAsync(string companyCen)
    {
        var entities = await _repository.GetByCompanyCenAsync(companyCen);
        return entities.Select(MapToDto);
    }

    public async Task<UnitResponseDto> CreateAsync(CreateUnitDto dto)
    {
        var entity = new Unit(dto.CompanyCen, dto.Name, dto.Abbreviation);
        await _repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task UpdateAsync(string unitCen, UpdateUnitDto dto)
    {
        var entity = await _repository.GetByCenAsync(unitCen);
        if (entity == null)
        {
            throw new InvalidOperationException("Unit not found");
        }

        typeof(Unit).GetProperty(nameof(Unit.UnitCen))?.SetValue(entity, unitCen);
        typeof(Unit).GetProperty(nameof(Unit.Name))?.SetValue(entity, dto.Name);
        typeof(Unit).GetProperty(nameof(Unit.Abbreviation))?.SetValue(entity, dto.Abbreviation);
        typeof(Unit).GetProperty(nameof(Unit.IsActive))?.SetValue(entity, dto.IsActive);

        await _repository.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    private static UnitResponseDto MapToDto(Unit entity) => new()
    {
        UnitCen = entity.UnitCen,
        CompanyCen = entity.CompanyCen,
        Name = entity.Name,
        Abbreviation = entity.Abbreviation,
        IsActive = entity.IsActive
    };
}
