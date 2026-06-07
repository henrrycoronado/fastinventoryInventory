namespace fastinventoryInventory.Src.Infraestructure.Persistence.Models;

public class StockModel
{
    public long Id { get; set; }
    public string StockCen { get; set; } = string.Empty;
    public string ProductCen { get; set; } = string.Empty;
    public string WarehouseCen { get; set; } = string.Empty;
    public decimal AvailableQuantity { get; set; }
    public decimal ReservedQuantity { get; set; }

    public ProductModel Product { get; set; } = null!;
    public WarehouseModel Warehouse { get; set; } = null!;
}
