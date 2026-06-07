using fastinventoryInventory.Src.Application.DTOs.Common;
using fastinventoryInventory.Src.Domain.Entities;
using fastinventoryInventory.Src.Infraestructure.Persistence.Interfaces;
using fastinventoryInventory.Src.Infraestructure.Persistence.Models;

using Microsoft.EntityFrameworkCore;

namespace fastinventoryInventory.Src.Infraestructure.Persistence.Repositories;

public class InventoryDocumentRepository : IInventoryDocumentRepository
{
    private readonly ApplicationDbContext _dbContext;

    public InventoryDocumentRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<InventoryDocument?> GetByCenAsync(string documentCen)
    {
        var model = await _dbContext.InventoryDocuments.Include(d => d.Lines).AsNoTracking().FirstOrDefaultAsync(d => d.DocumentCen == documentCen);
        return model == null ? null : MapToDomain(model);
    }

    public async Task<IEnumerable<InventoryDocument>> GetByCompanyCenAsync(string companyCen, InventoryDocumentQueryFilters? filters = null)
    {
        var query = _dbContext.InventoryDocuments.Include(d => d.Lines).Where(d => d.CompanyCen == companyCen);

        if (filters != null)
        {
            if (!string.IsNullOrEmpty(filters.DocumentType))
            {
                query = query.Where(d => d.DocumentType == filters.DocumentType);
            }

            if (filters.From.HasValue)
            {
                query = query.Where(d => d.CreatedAt >= filters.From.Value);
            }

            if (filters.To.HasValue)
            {
                query = query.Where(d => d.CreatedAt <= filters.To.Value);
            }
        }

        var models = await query.AsNoTracking().ToListAsync();
        return models.Select(MapToDomain);
    }

    public async Task AddAsync(InventoryDocument document)
    {
        var model = MapToModel(document);
        await _dbContext.InventoryDocuments.AddAsync(model);
    }

    public async Task UpdateAsync(InventoryDocument document)
    {
        var model = await _dbContext.InventoryDocuments.FirstOrDefaultAsync(d => d.DocumentCen == document.DocumentCen);
        if (model != null)
        {
            model.Status = document.Status;
            _dbContext.InventoryDocuments.Update(model);
        }
    }

    private static InventoryDocument MapToDomain(InventoryDocumentModel model)
    {
        var document = new InventoryDocument(model.CompanyCen, model.WarehouseCen, model.DocumentType, model.Title, model.ExternalReference);
        typeof(InventoryDocument).GetProperty(nameof(InventoryDocument.DocumentCen))?.SetValue(document, model.DocumentCen);
        typeof(InventoryDocument).GetProperty(nameof(InventoryDocument.Status))?.SetValue(document, model.Status);
        typeof(InventoryDocument).GetProperty(nameof(InventoryDocument.CreatedAt))?.SetValue(document, model.CreatedAt);
        typeof(InventoryDocument).GetProperty(nameof(InventoryDocument.Reason))?.SetValue(document, model.Reason);

        foreach (var line in model.Lines)
        {
            document.AddLine(line.ProductCen, line.Quantity, line.UnitCost);
            var addedLine = document.Lines.Last();
            typeof(InventoryDocumentLine).GetProperty(nameof(InventoryDocumentLine.LineCen))?.SetValue(addedLine, line.LineCen);
        }
        return document;
    }

    private static InventoryDocumentModel MapToModel(InventoryDocument entity)
    {
        var model = new InventoryDocumentModel
        {
            DocumentCen = entity.DocumentCen,
            CompanyCen = entity.CompanyCen,
            WarehouseCen = entity.WarehouseCen,
            DocumentType = entity.DocumentType,
            Status = entity.Status,
            Title = entity.Title,
            Reason = entity.Reason,
            ExternalReference = entity.ExternalReference,
            CreatedAt = entity.CreatedAt
        };

        foreach (var line in entity.Lines)
        {
            model.Lines.Add(new InventoryDocumentLineModel
            {
                LineCen = line.LineCen,
                DocumentCen = entity.DocumentCen,
                ProductCen = line.ProductCen,
                Quantity = line.Quantity,
                UnitCost = line.UnitCost
            });
        }
        return model;
    }
}
