namespace fastinventoryInventory.Src.Domain.Entities;

public class Warehouse
{
    public string WarehouseCen { get; private set; }
    public string CompanyCen { get; private set; }
    public string Name { get; private set; }
    public bool IsActive { get; private set; }

    public Warehouse(string companyCen, string name)
    {
        WarehouseCen = Guid.NewGuid().ToString("N");
        CompanyCen = companyCen;
        Name = name;
        IsActive = true;
    }

    public void Deactivate() => IsActive = false;
}
