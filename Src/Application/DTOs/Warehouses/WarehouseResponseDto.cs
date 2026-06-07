namespace fastinventoryInventory.Src.Application.DTOs.Warehouses;

public class WarehouseResponseDto
{
    public string WarehouseCen { get; set; } = string.Empty;
    public string CompanyCen { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
