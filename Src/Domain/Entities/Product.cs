namespace fastinventoryInventory.Src.Domain.Entities;

public class Product
{
    public string ProductCen { get; private set; }
    public string CompanyCen { get; private set; }
    public string? CategoryCen { get; private set; }
    public string? UnitCen { get; private set; }
    public string Sku { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public decimal SalePrice { get; private set; }
    public decimal CostPrice { get; private set; }
    public decimal ReorderLevel { get; private set; }
    public string Status { get; private set; }
    public string? StationCode { get; private set; }

    public Product(string companyCen, string sku, string name, decimal salePrice, decimal costPrice, decimal reorderLevel)
    {
        ProductCen = Guid.NewGuid().ToString("N");
        CompanyCen = companyCen;
        Sku = sku;
        Name = name;
        SalePrice = salePrice;
        CostPrice = costPrice;
        ReorderLevel = reorderLevel;
        Status = "ACTIVE";
    }

    public void AssignCategory(string categoryCen) => CategoryCen = categoryCen;
    public void AssignUnit(string unitCen) => UnitCen = unitCen;

    public void UpdatePrices(decimal salePrice, decimal costPrice)
    {
        if (salePrice < 0 || costPrice < 0)
            throw new ArgumentException("Los precios no pueden ser negativos.");

        SalePrice = salePrice;
        CostPrice = costPrice;
    }

    public void ChangeStatus(string newStatus) => Status = newStatus;
}
