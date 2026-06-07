namespace fastinventoryInventory.Src.Infraestructure.Persistence.Models;

public class CompanyModel
{
    public long Id { get; set; }
    public string CompanyCen { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public ICollection<WarehouseModel> Warehouses { get; set; } = new List<WarehouseModel>();
    public ICollection<CategoryModel> Categories { get; set; } = new List<CategoryModel>();
    public ICollection<UnitModel> Units { get; set; } = new List<UnitModel>();
    public ICollection<ProductModel> Products { get; set; } = new List<ProductModel>();
}
