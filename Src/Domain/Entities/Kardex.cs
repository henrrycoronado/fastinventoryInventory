namespace fastinventoryInventory.Src.Domain.Entities;

public class KardexMovement
{
    public string MovementCen { get; private set; }
    public string CompanyCen { get; private set; }
    public string WarehouseCen { get; private set; }
    public string ProductCen { get; private set; }
    public string? DocumentCen { get; private set; }
    public string MovementType { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal? UnitCost { get; private set; }
    public string? Reason { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public KardexMovement(string companyCen, string warehouseCen, string productCen, string movementType, decimal quantity, string? documentCen = null, decimal? unitCost = null, string? reason = null)
    {
        MovementCen = Guid.NewGuid().ToString("N");
        CompanyCen = companyCen;
        WarehouseCen = warehouseCen;
        ProductCen = productCen;
        DocumentCen = documentCen;
        MovementType = movementType;
        Quantity = quantity;
        UnitCost = unitCost;
        Reason = reason;
        CreatedAt = DateTimeOffset.UtcNow;
    }
}
