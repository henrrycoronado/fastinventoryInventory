using prismodInventory.Src.Application.DTOs.Common;
using prismodInventory.Src.Domain.Entities;

namespace prismodInventory.Src.Infraestructure.Persistence.Interfaces;

public interface IKardexMovementRepository
{
    Task<IEnumerable<KardexMovement>> GetByProductAndWarehouseAsync(string productCen, string warehouseCen, KardexQueryFilters? filters = null);
    Task<IEnumerable<KardexMovement>> GetByDocumentCenAsync(string documentCen);
    Task AddAsync(KardexMovement movement);
}
