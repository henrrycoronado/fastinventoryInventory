namespace prismodInventory.Src.Application.DTOs.Inventory;

public class CreateInventoryDocumentDto
{
    public string CompanyCen { get; set; } = string.Empty;
    public string WarehouseCen { get; set; } = string.Empty;
    public string DocumentType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Reason { get; set; }
    public string? ExternalReference { get; set; }
    public List<CreateDocumentLineDto> Lines { get; set; } = new();
}
