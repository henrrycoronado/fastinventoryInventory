namespace prismodInventory.Src.Application.DTOs.Inventory;

public class InventoryDashboardResponseDto
{
    public string CompanyCen { get; set; } = string.Empty;
    public int TotalProducts { get; set; }
    public decimal TotalStockQuantity { get; set; }
    public int LowStockCount { get; set; }
    public int OutOfStockCount { get; set; }
}
