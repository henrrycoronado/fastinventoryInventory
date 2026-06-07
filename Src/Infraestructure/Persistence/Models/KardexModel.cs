namespace prismodInventory.Src.Infraestructure.Persistence.Models;

public class KardexMovementModel
{
    public long Id { get; set; }
    public string MovementCen { get; set; } = string.Empty;
    public string CompanyCen { get; set; } = string.Empty;
    public string WarehouseCen { get; set; } = string.Empty;
    public string ProductCen { get; set; } = string.Empty;
    public string? DocumentCen { get; set; }
    public string MovementType { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal? UnitCost { get; set; }
    public string? Reason { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public CompanyModel Company { get; set; } = null!;
    public WarehouseModel Warehouse { get; set; } = null!;
    public ProductModel Product { get; set; } = null!;
    public InventoryDocumentModel? Document { get; set; }
}
