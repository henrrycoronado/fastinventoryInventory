namespace prismodInventory.Src.Domain.Entities;

public class InventoryDocument
{
    public string DocumentCen { get; private set; }
    public string CompanyCen { get; private set; }
    public string WarehouseCen { get; private set; }
    public string DocumentType { get; private set; }
    public string Status { get; private set; }
    public string Title { get; private set; }
    public string? Reason { get; private set; }
    public string? ExternalReference { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private readonly List<InventoryDocumentLine> _lines = new();
    public IReadOnlyCollection<InventoryDocumentLine> Lines => _lines.AsReadOnly();

    public InventoryDocument(string companyCen, string warehouseCen, string documentType, string title, string? externalReference = null)
    {
        DocumentCen = Guid.NewGuid().ToString("N");
        CompanyCen = companyCen;
        WarehouseCen = warehouseCen;
        DocumentType = documentType;
        Title = title;
        ExternalReference = externalReference;
        Status = "PENDING";
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void AddLine(string productCen, decimal quantity, decimal? unitCost = null)
    {
        _lines.Add(new InventoryDocumentLine(productCen, quantity, unitCost));
    }

    public void Complete() => Status = "COMPLETED";
}
