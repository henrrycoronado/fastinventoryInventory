namespace fastinventoryInventory.Src.Application.DTOs.Inventory;

public class DocumentLineResponseDto
{
    public string LineCen { get; set; } = string.Empty;
    public string ProductCen { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal? UnitCost { get; set; }
}
