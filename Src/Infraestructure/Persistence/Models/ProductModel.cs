namespace fastinventoryInventory.Src.Infraestructure.Persistence.Models;

public class ProductModel
{
    public long Id { get; set; }
    public string ProductCen { get; set; } = string.Empty;
    public string CompanyCen { get; set; } = string.Empty;
    public string? CategoryCen { get; set; }
    public string? UnitCen { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal SalePrice { get; set; }
    public decimal CostPrice { get; set; }
    public decimal ReorderLevel { get; set; }
    public string Status { get; set; } = "ACTIVE";
    public string? StationCode { get; set; }

    public CompanyModel Company { get; set; } = null!;
    public CategoryModel? Category { get; set; }
    public UnitModel? Unit { get; set; }
    public ICollection<StockModel> Stocks { get; set; } = new List<StockModel>();
}
