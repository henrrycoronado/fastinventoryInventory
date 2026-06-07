using Microsoft.AspNetCore.Mvc;

using prismodInventory.Src.Application.DTOs.Common;
using prismodInventory.Src.Application.DTOs.Inventory;
using prismodInventory.Src.Application.Interfaces;

namespace prismodInventory.Src.Presentation.Controllers;

[ApiController]
[Route("api/inventory/companies/{companyCen}")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet("dashboard")]
    public async Task<ActionResult<InventoryDashboardResponseDto>> GetDashboard(string companyCen)
    {
        var dashboard = await _inventoryService.GetDashboardAsync(companyCen);
        return Ok(dashboard);
    }

    [HttpGet("sellable-products")]
    public async Task<ActionResult<IEnumerable<SellableProductResponseDto>>> GetSellableProducts(
        string companyCen,
        [FromQuery] string? search,
        [FromQuery] string? categoryCen,
        [FromQuery] string? warehouseCen,
        [FromQuery] bool onlyAvailable = true,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        var filters = new SellableProductQueryFilters
        {
            Search = search,
            CategoryCen = categoryCen,
            WarehouseCen = warehouseCen,
            OnlyAvailable = onlyAvailable,
            Page = page,
            PageSize = pageSize
        };
        var products = await _inventoryService.GetSellableProductsAsync(companyCen, filters);
        return Ok(products);
    }

    [HttpGet("stock")]
    public async Task<ActionResult<IEnumerable<StockResponseDto>>> GetStock(
        string companyCen,
        [FromQuery] string? productCen,
        [FromQuery] string? warehouseCen)
    {
        if (string.IsNullOrEmpty(warehouseCen))
        {
            return Ok(new List<StockResponseDto>());
        }
        var stock = await _inventoryService.GetStockByWarehouseAsync(warehouseCen, productCen);
        return Ok(stock);
    }

    [HttpPost("stock/validate")]
    public async Task<ActionResult<StockValidationResponseDto>> ValidateStock(string companyCen, StockValidationRequestDto dto)
    {
        var result = await _inventoryService.ValidateStockAsync(dto);
        return Ok(result);
    }

    [HttpPost("documents")]
    public async Task<ActionResult<string>> CreateDocument(string companyCen, CreateInventoryDocumentDto dto)
    {
        dto.CompanyCen = companyCen;
        var documentCen = await _inventoryService.ProcessDocumentAsync(dto);
        return CreatedAtAction(nameof(GetDocument), new { companyCen, documentCen }, documentCen);
    }

    [HttpGet("documents")]
    public async Task<ActionResult<IEnumerable<InventoryDocumentResponseDto>>> GetDocuments(
        string companyCen,
        [FromQuery] string? documentType,
        [FromQuery] DateTimeOffset? from,
        [FromQuery] DateTimeOffset? to)
    {
        var filters = new InventoryDocumentQueryFilters
        {
            DocumentType = documentType,
            From = from,
            To = to
        };
        var documents = await _inventoryService.GetDocumentsByCompanyAsync(companyCen, filters);
        return Ok(documents);
    }

    [HttpGet("documents/{documentCen}")]
    public async Task<ActionResult<InventoryDocumentResponseDto>> GetDocument(string companyCen, string documentCen)
    {
        var document = await _inventoryService.GetDocumentByCenAsync(documentCen);
        if (document == null) return NotFound();
        return Ok(document);
    }

    [HttpGet("products/{productCen}/kardex")]
    public async Task<ActionResult<IEnumerable<KardexMovementResponseDto>>> GetKardex(
        string companyCen,
        string productCen,
        [FromQuery] string warehouseCen,
        [FromQuery] DateTimeOffset? from,
        [FromQuery] DateTimeOffset? to)
    {
        var filters = new KardexQueryFilters
        {
            From = from,
            To = to
        };
        var movements = await _inventoryService.GetKardexByProductAndWarehouseAsync(productCen, warehouseCen, filters);
        return Ok(movements);
    }

    [HttpPost("stock/consume")]
    public async Task<ActionResult<StockConsumeResponseDto>> ConsumeStock(string companyCen, StockValidationRequestDto dto)
    {
        var result = await _inventoryService.ConsumeStockAsync(companyCen, dto);
        return Ok(result);
    }

    [HttpPost("stock/increase")]
    public async Task<ActionResult<string>> IncreaseStock(string companyCen, StockValidationRequestDto dto)
    {
        var documentCen = await _inventoryService.IncreaseStockAsync(companyCen, dto);
        return Ok(documentCen);
    }

    [HttpPost("stock/adjustments")]
    public async Task<ActionResult<InventoryAdjustmentContractResponse>> AdjustStock(string companyCen, InventoryAdjustmentRequestDto dto)
    {
        var result = await _inventoryService.AdjustStockAsync(companyCen, dto);
        return Ok(result);
    }
}
