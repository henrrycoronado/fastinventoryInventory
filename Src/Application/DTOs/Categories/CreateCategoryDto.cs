namespace prismodInventory.Src.Application.DTOs.Categories;

public class CreateCategoryDto
{
    public string CompanyCen { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
