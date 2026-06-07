namespace fastinventoryInventory.Src.Domain.Entities;

public class Category
{
    public string CategoryCen { get; private set; }
    public string CompanyCen { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }

    public Category(string companyCen, string name, string? description = null)
    {
        CategoryCen = Guid.NewGuid().ToString("N");
        CompanyCen = companyCen;
        Name = name;
        Description = description;
        IsActive = true;
    }
}
