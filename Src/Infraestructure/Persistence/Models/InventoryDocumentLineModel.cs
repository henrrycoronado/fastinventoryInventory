namespace fastinventoryInventory.Src.Infraestructure.Persistence.Models;

public class InventoryDocumentLineModel
{
    public long Id { get; set; }
    public string LineCen { get; set; } = string.Empty;
    public string DocumentCen { get; set; } = string.Empty;
    public string ProductCen { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal? UnitCost { get; set; }

    public InventoryDocumentModel Document { get; set; } = null!;
    public ProductModel Product { get; set; } = null!;
}
