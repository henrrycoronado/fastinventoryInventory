using fastinventoryInventory.Src.Application.DTOs.Units;
using fastinventoryInventory.Src.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace fastinventoryInventory.Src.Presentation.Controllers;

[ApiController]
[Route("api/inventory/companies/{companyCen}/units")]
public class UnitsController : ControllerBase
{
    private readonly IUnitService _unitService;

    public UnitsController(IUnitService unitService)
    {
        _unitService = unitService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UnitResponseDto>>> GetByCompany(string companyCen)
    {
        var units = await _unitService.GetByCompanyAsync(companyCen);
        return Ok(units);
    }

    [HttpPost]
    public async Task<ActionResult<UnitResponseDto>> Create(string companyCen, CreateUnitDto dto)
    {
        dto.CompanyCen = companyCen;
        var unit = await _unitService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetByCompany), new { companyCen = companyCen }, unit);
    }

    [HttpPut("{unitCen}")]
    public async Task<IActionResult> Update(string unitCen, UpdateUnitDto dto)
    {
        await _unitService.UpdateAsync(unitCen, dto);
        return NoContent();
    }
}
