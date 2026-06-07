namespace prismodInventory.Src.Application.DTOs.Inventory;

public class SellableProductResponseDto
{
    public string ProductCen { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? CategoryCen { get; set; }
    public string? CategoryName { get; set; }
    public decimal SalePrice { get; set; }
    public decimal AvailableQuantity { get; set; }
    public bool IsAvailable { get; set; }
    public string? StationCode { get; set; }
}
