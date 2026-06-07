namespace prismodInventory.Src.Domain.Entities;

public class Company
{
    public string CompanyCen { get; private set; }
    public string Name { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public Company(string name)
    {
        CompanyCen = Guid.NewGuid().ToString("N");
        Name = name;
        IsActive = true;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
