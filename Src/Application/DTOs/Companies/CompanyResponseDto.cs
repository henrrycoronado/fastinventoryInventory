namespace fastinventoryInventory.Src.Application.DTOs.Companies;

public class CompanyResponseDto
{
    public string CompanyCen { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
