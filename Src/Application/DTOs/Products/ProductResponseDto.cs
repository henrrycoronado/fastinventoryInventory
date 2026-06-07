namespace fastinventoryInventory.Src.Application.DTOs.Products;

public class ProductResponseDto
{
    public string ProductCen { get; set; } = string.Empty;
    public string CompanyCen { get; set; } = string.Empty;
    public string? CategoryCen { get; set; }
    public string? CategoryName { get; set; }
    public string? UnitCen { get; set; }
    public string? UnitName { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal SalePrice { get; set; }
    public decimal CostPrice { get; set; }
    public decimal ReorderLevel { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? StationCode { get; set; }
}
