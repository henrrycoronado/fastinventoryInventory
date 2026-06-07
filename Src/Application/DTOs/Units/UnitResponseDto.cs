namespace fastinventoryInventory.Src.Application.DTOs.Units;

public class UnitResponseDto
{
    public string UnitCen { get; set; } = string.Empty;
    public string CompanyCen { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Abbreviation { get; set; }
    public bool IsActive { get; set; }
}
