using fastinventoryInventory.Src.Application.DTOs.Common;
using fastinventoryInventory.Src.Application.DTOs.Inventory;

namespace fastinventoryInventory.Src.Application.Interfaces;

public interface IInventoryService
{
    Task<InventoryDocumentResponseDto> ProcessDocumentAsync(CreateInventoryDocumentDto dto);
    Task<InventoryAdjustmentContractResponse> AdjustStockAsync(string companyCen, InventoryAdjustmentRequestDto dto);
    Task<StockConsumeResponseDto> ConsumeStockAsync(string companyCen, StockValidationRequestDto dto);
    Task<string> IncreaseStockAsync(string companyCen, StockValidationRequestDto dto);

    Task<StockResponseDto?> GetStockAsync(string productCen, string warehouseCen);
    Task<IEnumerable<StockResponseDto>> GetStockByWarehouseAsync(string warehouseCen, string? productCen = null);
    Task<StockValidationResponseDto> ValidateStockAsync(StockValidationRequestDto dto);

    Task<InventoryDocumentResponseDto?> GetDocumentByCenAsync(string documentCen);
    Task<IEnumerable<InventoryDocumentResponseDto>> GetDocumentsByCompanyAsync(string companyCen, InventoryDocumentQueryFilters? filters = null);

    Task<IEnumerable<KardexMovementResponseDto>> GetKardexByProductAndWarehouseAsync(string productCen, string warehouseCen, KardexQueryFilters? filters = null);
    Task<IEnumerable<KardexMovementResponseDto>> GetKardexByDocumentCenAsync(string documentCen);

    Task<InventoryDashboardResponseDto> GetDashboardAsync(string companyCen);
    Task<IEnumerable<SellableProductResponseDto>> GetSellableProductsAsync(string companyCen, SellableProductQueryFilters? filters = null);
}
