namespace prismodInventory.Src.Application.DTOs.Inventory;

public class KardexMovementResponseDto
{
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
}
