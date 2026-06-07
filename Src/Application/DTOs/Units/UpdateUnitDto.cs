namespace prismodInventory.Src.Application.DTOs.Units;

public class UpdateUnitDto
{
    public string Name { get; set; } = string.Empty;
    public string? Abbreviation { get; set; }
    public bool IsActive { get; set; }
}
