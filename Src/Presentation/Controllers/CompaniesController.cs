using fastinventoryInventory.Src.Application.DTOs.Companies;
using fastinventoryInventory.Src.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace fastinventoryInventory.Src.Presentation.Controllers;

[ApiController]
[Route("api/inventory/companies")]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CompanyResponseDto>>> GetAll()
    {
        var companies = await _companyService.GetAllAsync();
        return Ok(companies);
    }

    [HttpGet("{companyCen}")]
    public async Task<ActionResult<CompanyLookupResponseDto>> GetByCen(string companyCen)
    {
        var company = await _companyService.GetLookupByCenAsync(companyCen);
        if (company == null)
        {
            return NotFound();
        }
        return Ok(company);
    }

    [HttpPost]
    public async Task<ActionResult<CompanyResponseDto>> Create(CreateCompanyDto dto)
    {
        var company = await _companyService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetByCen), new { companyCen = company.CompanyCen }, company);
    }

    [HttpPut("{companyCen}")]
    public async Task<IActionResult> Update(string companyCen, UpdateCompanyDto dto)
    {
        await _companyService.UpdateAsync(companyCen, dto);
        return NoContent();
    }
}
