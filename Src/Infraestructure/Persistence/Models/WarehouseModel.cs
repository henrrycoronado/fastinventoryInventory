namespace prismodInventory.Src.Infraestructure.Persistence.Models;

public class WarehouseModel
{
    public long Id { get; set; }
    public string WarehouseCen { get; set; } = string.Empty;
    public string CompanyCen { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public CompanyModel Company { get; set; } = null!;
    public ICollection<StockModel> Stocks { get; set; } = new List<StockModel>();
}
