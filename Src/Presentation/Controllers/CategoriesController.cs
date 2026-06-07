using Microsoft.AspNetCore.Mvc;

using prismodInventory.Src.Application.DTOs.Categories;
using prismodInventory.Src.Application.Interfaces;

namespace prismodInventory.Src.Presentation.Controllers;

[ApiController]
[Route("api/inventory/companies/{companyCen}/categories")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetByCompany(string companyCen)
    {
        var categories = await _categoryService.GetByCompanyAsync(companyCen);
        return Ok(categories);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponseDto>> Create(string companyCen, CreateCategoryDto dto)
    {
        dto.CompanyCen = companyCen;
        var category = await _categoryService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetByCompany), new { companyCen = companyCen }, category);
    }

    [HttpPut("{categoryCen}")]
    public async Task<IActionResult> Update(string categoryCen, UpdateCategoryDto dto)
    {
        await _categoryService.UpdateAsync(categoryCen, dto);
        return NoContent();
    }
}
