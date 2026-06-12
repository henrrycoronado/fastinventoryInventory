namespace fastinventoryInventory.Src.Application.DTOs.Inventory;

public class RestockEvent
{
    public string ProductCen { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string WarehouseCen { get; set; } = string.Empty;
    public string WarehouseName { get; set; } = string.Empty;
    public DateTimeOffset OccurredAt { get; set; } = DateTimeOffset.UtcNow;
}
