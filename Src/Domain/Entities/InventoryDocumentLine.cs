namespace fastinventoryInventory.Src.Domain.Entities;

public class InventoryDocumentLine
{
    public string LineCen { get; private set; }
    public string ProductCen { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal? UnitCost { get; private set; }

    public InventoryDocumentLine(string productCen, decimal quantity, decimal? unitCost = null)
    {
        LineCen = Guid.NewGuid().ToString("N");
        ProductCen = productCen;
        Quantity = quantity;
        UnitCost = unitCost;
    }
}
