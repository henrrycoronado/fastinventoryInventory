using prismodInventory.Src.Application.DTOs.Common;
using prismodInventory.Src.Domain.Entities;

namespace prismodInventory.Src.Infraestructure.Persistence.Interfaces;

public interface IInventoryDocumentRepository
{
    Task<InventoryDocument?> GetByCenAsync(string documentCen);
    Task<IEnumerable<InventoryDocument>> GetByCompanyCenAsync(string companyCen, InventoryDocumentQueryFilters? filters = null);
    Task AddAsync(InventoryDocument document);
    Task UpdateAsync(InventoryDocument document);
}
