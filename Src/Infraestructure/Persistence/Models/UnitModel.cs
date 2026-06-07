namespace fastinventoryInventory.Src.Infraestructure.Persistence.Models;

public class UnitModel
{
    public long Id { get; set; }
    public string UnitCen { get; set; } = string.Empty;
    public string CompanyCen { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Abbreviation { get; set; }
    public bool IsActive { get; set; }

    public CompanyModel Company { get; set; } = null!;
    public ICollection<ProductModel> Products { get; set; } = new List<ProductModel>();
}
