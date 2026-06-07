namespace fastinventoryInventory.Src.Application.DTOs.Products;

public class CreateProductDto
{
    public string CompanyCen { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal SalePrice { get; set; }
    public decimal CostPrice { get; set; }
    public decimal ReorderLevel { get; set; }
    public string? CategoryCen { get; set; }
    public string? UnitCen { get; set; }
    public string? StationCode { get; set; }
}
