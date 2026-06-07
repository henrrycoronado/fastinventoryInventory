using Microsoft.AspNetCore.Mvc;

using prismodInventory.Src.Application.DTOs.Common;
using prismodInventory.Src.Application.DTOs.Inventory;
using prismodInventory.Src.Application.DTOs.Products;
using prismodInventory.Src.Application.Interfaces;

namespace prismodInventory.Src.Presentation.Controllers;

[ApiController]
[Route("api/inventory/companies/{companyCen}/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetByCompany(
        string companyCen,
        [FromQuery] string? search,
        [FromQuery] string? categoryCen,
        [FromQuery] string? status)
    {
        var filters = new ProductQueryFilters
        {
            Search = search,
            CategoryCen = categoryCen,
            Status = status
        };
        var products = await _productService.GetByCompanyAsync(companyCen, filters);
        return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult<CreateProductResponseDto>> Create(string companyCen, CreateProductDto dto)
    {
        dto.CompanyCen = companyCen;
        var product = await _productService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetByCompany), new { companyCen = companyCen }, product);
    }

    [HttpPost("lookup")]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> Lookup(string companyCen, ProductLookupRequestDto dto)
    {
        var products = await _productService.GetByCensAsync(dto.ProductCens);
        return Ok(products);
    }

    [HttpPut("{productCen}")]
    public async Task<IActionResult> Update(string productCen, UpdateProductDto dto)
    {
        await _productService.UpdateAsync(productCen, dto);
        return NoContent();
    }

    [HttpPatch("{productCen}/status")]
    public async Task<IActionResult> UpdateStatus(string productCen, UpdateProductStatusDto dto)
    {
        await _productService.UpdateStatusAsync(productCen, dto);
        return NoContent();
    }
}
