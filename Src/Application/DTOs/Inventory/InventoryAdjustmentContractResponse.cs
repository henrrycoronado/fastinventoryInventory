namespace prismodInventory.Src.Application.DTOs.Inventory;

public class GeneratedMovementContractDto
{
    public string MovementCen { get; set; } = string.Empty;
    public string ProductCen { get; set; } = string.Empty;
    public string WarehouseCen { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string MovementType { get; set; } = string.Empty;
}

public class InventoryAdjustmentContractResponse
{
    public string AdjustmentCen { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<GeneratedMovementContractDto> GeneratedMovements { get; set; } = new();
}
