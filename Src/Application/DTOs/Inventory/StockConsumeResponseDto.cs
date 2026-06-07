namespace prismodInventory.Src.Application.DTOs.Inventory;

public class StockConsumeResponseDto
{
    public bool Success { get; set; }
    public string? DocumentCen { get; set; }
    public string? DocumentType { get; set; }
    public List<string> GeneratedMovementCens { get; set; } = new();
    public List<StockRequirementDto> Requirements { get; set; } = new();
}
