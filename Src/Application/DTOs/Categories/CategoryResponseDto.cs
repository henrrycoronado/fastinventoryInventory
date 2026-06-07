namespace prismodInventory.Src.Application.DTOs.Categories;

public class CategoryResponseDto
{
    public string CategoryCen { get; set; } = string.Empty;
    public string CompanyCen { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
