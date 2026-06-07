namespace fastinventoryInventory.Src.Application.DTOs.Inventory;

public class CreateDocumentLineDto
{
    public string ProductCen { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal? UnitCost { get; set; }
}
