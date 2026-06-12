using fastinventoryInventory.Src.Application.DTOs.Common;
using fastinventoryInventory.Src.Application.DTOs.Inventory;
using fastinventoryInventory.Src.Application.DTOs.Products;
using fastinventoryInventory.Src.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace fastinventoryInventory.Src.Presentation.Controllers;

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
    public async Task<ActionResult<ProductResponseDto>> Update(string productCen, UpdateProductDto dto)
    {
        var product = await _productService.UpdateAsync(productCen, dto);
        return Ok(product);
    }

    [HttpPatch("{productCen}/status")]
    public async Task<ActionResult<ProductResponseDto>> UpdateStatus(string productCen, UpdateProductStatusDto dto)
    {
        var product = await _productService.UpdateStatusAsync(productCen, dto);
        return Ok(product);
    }
}
