namespace prismodInventory.Src.Domain.Entities;

public class Unit
{
    public string UnitCen { get; private set; }
    public string CompanyCen { get; private set; }
    public string Name { get; private set; }
    public string? Abbreviation { get; private set; }
    public bool IsActive { get; private set; }

    public Unit(string companyCen, string name, string? abbreviation = null)
    {
        UnitCen = Guid.NewGuid().ToString("N");
        CompanyCen = companyCen;
        Name = name;
        Abbreviation = abbreviation;
        IsActive = true;
    }
}
