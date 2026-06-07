namespace fastinventoryInventory.Src.Application.DTOs.Common;

public class ProductQueryFilters
{
    public string? Search { get; set; }
    public string? CategoryCen { get; set; }
    public string? Status { get; set; }
}

public class SellableProductQueryFilters : ProductQueryFilters
{
    public string? WarehouseCen { get; set; }
    public bool OnlyAvailable { get; set; } = true;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}

public class InventoryDocumentQueryFilters
{
    public string? DocumentType { get; set; }
    public DateTimeOffset? From { get; set; }
    public DateTimeOffset? To { get; set; }
}

public class KardexQueryFilters
{
    public DateTimeOffset? From { get; set; }
    public DateTimeOffset? To { get; set; }
}
