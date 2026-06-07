namespace fastinventoryInventory.Src.Application.DTOs.Inventory;

public class InventoryDocumentResponseDto
{
    public string DocumentCen { get; set; } = string.Empty;
    public string CompanyCen { get; set; } = string.Empty;
    public string WarehouseCen { get; set; } = string.Empty;
    public string DocumentType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Reason { get; set; }
    public string? ExternalReference { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public int TotalItems { get; set; }
    public List<string> GeneratedMovementCens { get; set; } = new();
    public List<DocumentLineResponseDto> Lines { get; set; } = new();
}
