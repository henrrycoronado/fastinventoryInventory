namespace prismodInventory.Src.Domain.Entities;

public class Stock
{
    public string StockCen { get; private set; }
    public string ProductCen { get; private set; }
    public string WarehouseCen { get; private set; }
    public decimal AvailableQuantity { get; private set; }
    public decimal ReservedQuantity { get; private set; }

    public Stock(string productCen, string warehouseCen, decimal initialQuantity = 0)
    {
        StockCen = Guid.NewGuid().ToString("N");
        ProductCen = productCen;
        WarehouseCen = warehouseCen;
        AvailableQuantity = initialQuantity;
        ReservedQuantity = 0;
    }

    public void Increase(decimal amount)
    {
        if (amount <= 0) throw new ArgumentException("La cantidad debe ser mayor a 0.");
        AvailableQuantity += amount;
    }

    public void Consume(decimal amount)
    {
        if (amount <= 0) throw new ArgumentException("La cantidad debe ser mayor a 0.");
        if (AvailableQuantity < amount) throw new InvalidOperationException("Stock insuficiente.");

        AvailableQuantity -= amount;
    }
}
