namespace fastinventoryInventory.Src.Application.DTOs.Inventory;

public class ProductLookupRequestDto
{
    public List<string> ProductCens { get; set; } = new();
}

public class InventoryMovementItemDto
{
    public string ProductCen { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
}

public class InventoryMovementRequestDto
{
    public string WarehouseCen { get; set; } = string.Empty;
    public string? Source { get; set; }
    public string? ReferenceCen { get; set; }
    public string? Reason { get; set; }
    public List<InventoryMovementItemDto> Items { get; set; } = new();
}

public class InventoryAdjustmentLineDto
{
    public string ProductCen { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string AdjustmentType { get; set; } = string.Empty;
}

public class InventoryAdjustmentRequestDto
{
    public string WarehouseCen { get; set; } = string.Empty;
    public string? Reason { get; set; }
    public List<InventoryAdjustmentLineDto> Lines { get; set; } = new();
}

public class StockValidationItemDto
{
    public string ProductCen { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
}

public class StockValidationRequestDto
{
    public string WarehouseCen { get; set; } = string.Empty;
    public string? Source { get; set; }
    public string? ReferenceCen { get; set; }
    public string? Reason { get; set; }
    public List<StockValidationItemDto> Items { get; set; } = new();
}

public class StockRequirementDto
{
    public string ProductCen { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string WarehouseCen { get; set; } = string.Empty;
    public decimal RequestedQuantity { get; set; }
    public decimal AvailableQuantity { get; set; }
    public decimal MissingQuantity { get; set; }
    public string UnitName { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
}

public class StockValidationResponseDto
{
    public bool IsValid { get; set; }
    public List<StockRequirementDto> Requirements { get; set; } = new();
}
