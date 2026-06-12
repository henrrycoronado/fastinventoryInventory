using fastinventoryInventory.Src.Application.DTOs.Warehouses;
using fastinventoryInventory.Src.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace fastinventoryInventory.Src.Presentation.Controllers;

[ApiController]
[Route("api/inventory/companies/{companyCen}/warehouses")]
public class WarehousesController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;

    public WarehousesController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WarehouseResponseDto>>> GetByCompany(string companyCen)
    {
        var warehouses = await _warehouseService.GetByCompanyAsync(companyCen);
        return Ok(warehouses);
    }

    [HttpPost]
    public async Task<ActionResult<WarehouseResponseDto>> Create(string companyCen, CreateWarehouseDto dto)
    {
        dto.CompanyCen = companyCen;
        var warehouse = await _warehouseService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetByCompany), new { companyCen = companyCen }, warehouse);
    }

    [HttpPut("{warehouseCen}")]
    public async Task<ActionResult<WarehouseResponseDto>> Update(string warehouseCen, UpdateWarehouseDto dto)
    {
        var warehouse = await _warehouseService.UpdateAsync(warehouseCen, dto);
        return Ok(warehouse);
    }
}
