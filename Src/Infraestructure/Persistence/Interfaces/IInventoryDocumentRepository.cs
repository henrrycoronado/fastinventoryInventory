using fastinventoryInventory.Src.Application.DTOs.Common;
using fastinventoryInventory.Src.Domain.Entities;

namespace fastinventoryInventory.Src.Infraestructure.Persistence.Interfaces;

public interface IInventoryDocumentRepository
{
    Task<InventoryDocument?> GetByCenAsync(string documentCen);
    Task<IEnumerable<InventoryDocument>> GetByCompanyCenAsync(string companyCen, InventoryDocumentQueryFilters? filters = null);
    Task AddAsync(InventoryDocument document);
    Task UpdateAsync(InventoryDocument document);
}
