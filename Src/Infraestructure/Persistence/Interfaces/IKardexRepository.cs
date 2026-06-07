using fastinventoryInventory.Src.Application.DTOs.Common;
using fastinventoryInventory.Src.Domain.Entities;

namespace fastinventoryInventory.Src.Infraestructure.Persistence.Interfaces;

public interface IKardexMovementRepository
{
    Task<IEnumerable<KardexMovement>> GetByProductAndWarehouseAsync(string productCen, string warehouseCen, KardexQueryFilters? filters = null);
    Task<IEnumerable<KardexMovement>> GetByDocumentCenAsync(string documentCen);
    Task AddAsync(KardexMovement movement);
}
