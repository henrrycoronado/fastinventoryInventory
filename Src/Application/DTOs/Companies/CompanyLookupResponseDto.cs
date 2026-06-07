namespace prismodInventory.Src.Application.DTOs.Companies;

public class CompanyLookupResponseDto
{
    public int CompanyId { get; set; }
    public string CompanyCen { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
