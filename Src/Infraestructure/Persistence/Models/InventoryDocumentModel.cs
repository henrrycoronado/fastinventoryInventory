namespace fastinventoryInventory.Src.Infraestructure.Persistence.Models;

public class InventoryDocumentModel
{
    public long Id { get; set; }
    public string DocumentCen { get; set; } = string.Empty;
    public string CompanyCen { get; set; } = string.Empty;
    public string WarehouseCen { get; set; } = string.Empty;
    public string DocumentType { get; set; } = string.Empty;
    public string Status { get; set; } = "COMPLETED";
    public string Title { get; set; } = string.Empty;
    public string? Reason { get; set; }
    public string? ExternalReference { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public CompanyModel Company { get; set; } = null!;
    public WarehouseModel Warehouse { get; set; } = null!;
    public ICollection<InventoryDocumentLineModel> Lines { get; set; } = new List<InventoryDocumentLineModel>();
}
