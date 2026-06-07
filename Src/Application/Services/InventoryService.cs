using prismodInventory.Src.Application.DTOs.Common;
using prismodInventory.Src.Application.DTOs.Inventory;
using prismodInventory.Src.Application.Interfaces;
using prismodInventory.Src.Domain.Entities;
using prismodInventory.Src.Infraestructure.Persistence.Interfaces;

namespace prismodInventory.Src.Application.Services;

public class InventoryService : IInventoryService
{
    private readonly IInventoryDocumentRepository _documentRepository;
    private readonly IStockRepository _stockRepository;
    private readonly IKardexMovementRepository _kardexRepository;
    private readonly IProductRepository _productRepository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IUnitRepository _unitRepository;
    private readonly IUnitOfWork _unitOfWork;

    public InventoryService(
        IInventoryDocumentRepository documentRepository,
        IStockRepository stockRepository,
        IKardexMovementRepository kardexRepository,
        IProductRepository productRepository,
        IWarehouseRepository warehouseRepository,
        IUnitRepository unitRepository,
        IUnitOfWork unitOfWork)
    {
        _documentRepository = documentRepository;
        _stockRepository = stockRepository;
        _kardexRepository = kardexRepository;
        _productRepository = productRepository;
        _warehouseRepository = warehouseRepository;
        _unitRepository = unitRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> ProcessDocumentAsync(CreateInventoryDocumentDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var document = new InventoryDocument(dto.CompanyCen, dto.WarehouseCen, dto.DocumentType, dto.Title, dto.ExternalReference);
            typeof(InventoryDocument).GetProperty(nameof(InventoryDocument.Reason))?.SetValue(document, dto.Reason);

            var normalizedType = dto.DocumentType.Trim().ToUpperInvariant();
            var isInput = normalizedType is "INCOME" or "ADJUSTMENT_IN";
            var isOutput = normalizedType is "OUTCOME" or "ADJUSTMENT_OUT";

            foreach (var line in dto.Lines)
            {
                document.AddLine(line.ProductCen, line.Quantity, line.UnitCost);

                var stock = await _stockRepository.GetAsync(line.ProductCen, dto.WarehouseCen);
                if (stock == null)
                {
                    stock = new Stock(line.ProductCen, dto.WarehouseCen, 0);
                    await _stockRepository.AddAsync(stock);
                }

                if (isInput)
                {
                    stock.Increase(line.Quantity);
                }
                else if (isOutput)
                {
                    stock.Consume(line.Quantity);
                }
                else
                {
                    throw new InvalidOperationException("Document type not supported");
                }

                await _stockRepository.UpdateAsync(stock);

                var movementType = isInput ? "INPUT" : "OUTPUT";
                var movement = new KardexMovement(
                    dto.CompanyCen,
                    dto.WarehouseCen,
                    line.ProductCen,
                    movementType,
                    line.Quantity,
                    document.DocumentCen,
                    line.UnitCost);

                await _kardexRepository.AddAsync(movement);
            }

            document.Complete();
            await _documentRepository.AddAsync(document);
            await _unitOfWork.CommitTransactionAsync();
            return document.DocumentCen;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<InventoryAdjustmentContractResponse> AdjustStockAsync(string companyCen, InventoryAdjustmentRequestDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var document = new InventoryDocument(companyCen, dto.WarehouseCen, "ADJUSTMENT", "Manual Adjustment", null);
            typeof(InventoryDocument).GetProperty(nameof(InventoryDocument.Reason))?.SetValue(document, dto.Reason);

            var generatedMovements = new List<GeneratedMovementContractDto>();

            foreach (var line in dto.Lines)
            {
                document.AddLine(line.ProductCen, line.Quantity, null);

                var stock = await _stockRepository.GetAsync(line.ProductCen, dto.WarehouseCen);
                if (stock == null)
                {
                    stock = new Stock(line.ProductCen, dto.WarehouseCen, 0);
                    await _stockRepository.AddAsync(stock);
                }

                var isInput = line.AdjustmentType.Trim().ToUpperInvariant() == "IN";
                if (isInput)
                {
                    stock.Increase(line.Quantity);
                }
                else
                {
                    stock.Consume(line.Quantity);
                }

                await _stockRepository.UpdateAsync(stock);

                var movementType = isInput ? "INPUT" : "OUTPUT";
                var movement = new KardexMovement(
                    companyCen,
                    dto.WarehouseCen,
                    line.ProductCen,
                    movementType,
                    line.Quantity,
                    document.DocumentCen,
                    null);

                await _kardexRepository.AddAsync(movement);

                generatedMovements.Add(new GeneratedMovementContractDto
                {
                    MovementCen = movement.MovementCen,
                    ProductCen = movement.ProductCen,
                    WarehouseCen = movement.WarehouseCen,
                    Quantity = movement.Quantity,
                    MovementType = movement.MovementType
                });
            }

            document.Complete();
            await _documentRepository.AddAsync(document);
            await _unitOfWork.CommitTransactionAsync();

            return new InventoryAdjustmentContractResponse
            {
                AdjustmentCen = document.DocumentCen,
                Status = document.Status,
                GeneratedMovements = generatedMovements
            };
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<StockConsumeResponseDto> ConsumeStockAsync(string companyCen, StockValidationRequestDto dto)
    {
        var validation = await ValidateStockAsync(dto);
        if (!validation.IsValid)
        {
            return new StockConsumeResponseDto
            {
                Success = false,
                Requirements = validation.Requirements
            };
        }

        var docDto = new CreateInventoryDocumentDto
        {
            CompanyCen = companyCen,
            WarehouseCen = dto.WarehouseCen,
            DocumentType = "OUTCOME",
            Title = $"Stock Consume - {dto.Source}",
            ExternalReference = dto.ReferenceCen,
            Lines = dto.Items.Select(i => new CreateDocumentLineDto
            {
                ProductCen = i.ProductCen,
                Quantity = i.Quantity
            }).ToList()
        };

        var documentCen = await ProcessDocumentAsync(docDto);
        var movements = await _kardexRepository.GetByDocumentCenAsync(documentCen);

        return new StockConsumeResponseDto
        {
            Success = true,
            DocumentCen = documentCen,
            DocumentType = "OUTCOME",
            GeneratedMovementCens = movements.Select(m => m.MovementCen).ToList()
        };
    }

    public async Task<string> IncreaseStockAsync(string companyCen, StockValidationRequestDto dto)
    {
        var docDto = new CreateInventoryDocumentDto
        {
            CompanyCen = companyCen,
            WarehouseCen = dto.WarehouseCen,
            DocumentType = "INCOME",
            Title = $"Stock Increase - {dto.Source}",
            ExternalReference = dto.ReferenceCen,
            Lines = dto.Items.Select(i => new CreateDocumentLineDto
            {
                ProductCen = i.ProductCen,
                Quantity = i.Quantity
            }).ToList()
        };

        return await ProcessDocumentAsync(docDto);
    }

    public async Task<StockResponseDto?> GetStockAsync(string productCen, string warehouseCen)
    {
        var stock = await _stockRepository.GetAsync(productCen, warehouseCen);
        return stock == null ? null : await MapToDto(stock);
    }

    public async Task<IEnumerable<StockResponseDto>> GetStockByWarehouseAsync(string warehouseCen, string? productCen = null)
    {
        var stocks = await _stockRepository.GetByWarehouseCenAsync(warehouseCen, productCen);
        var dtos = new List<StockResponseDto>();
        foreach (var stock in stocks)
        {
            dtos.Add(await MapToDto(stock));
        }
        return dtos;
    }

    public async Task<InventoryDocumentResponseDto?> GetDocumentByCenAsync(string documentCen)
    {
        var document = await _documentRepository.GetByCenAsync(documentCen);
        if (document == null) return null;

        var dto = MapToDto(document);
        var movements = await _kardexRepository.GetByDocumentCenAsync(documentCen);
        dto.GeneratedMovementCens = movements.Select(m => m.MovementCen).ToList();
        dto.TotalItems = document.Lines.Count;

        return dto;
    }

    public async Task<IEnumerable<InventoryDocumentResponseDto>> GetDocumentsByCompanyAsync(string companyCen, InventoryDocumentQueryFilters? filters = null)
    {
        var documents = await _documentRepository.GetByCompanyCenAsync(companyCen, filters);
        var dtos = new List<InventoryDocumentResponseDto>();
        foreach (var doc in documents)
        {
            var dto = MapToDto(doc);
            dto.TotalItems = doc.Lines.Count;
            dtos.Add(dto);
        }
        return dtos;
    }

    public async Task<IEnumerable<KardexMovementResponseDto>> GetKardexByProductAndWarehouseAsync(string productCen, string warehouseCen, KardexQueryFilters? filters = null)
    {
        var movements = await _kardexRepository.GetByProductAndWarehouseAsync(productCen, warehouseCen, filters);
        return movements.Select(MapToDto);
    }

    public async Task<IEnumerable<KardexMovementResponseDto>> GetKardexByDocumentCenAsync(string documentCen)
    {
        var movements = await _kardexRepository.GetByDocumentCenAsync(documentCen);
        return movements.Select(MapToDto);
    }

    public async Task<InventoryDashboardResponseDto> GetDashboardAsync(string companyCen)
    {
        var products = await _productRepository.GetByCompanyCenAsync(companyCen);
        var stocks = new List<Stock>();
        foreach (var product in products)
        {
            var productStocks = await _stockRepository.GetByProductCenAsync(product.ProductCen);
            stocks.AddRange(productStocks);
        }

        return new InventoryDashboardResponseDto
        {
            CompanyCen = companyCen,
            TotalProducts = products.Count(),
            TotalStockQuantity = stocks.Sum(s => s.AvailableQuantity),
            LowStockCount = products.Count(p => stocks.Where(s => s.ProductCen == p.ProductCen).Sum(s => s.AvailableQuantity) <= p.ReorderLevel),
            OutOfStockCount = products.Count(p => stocks.Where(s => s.ProductCen == p.ProductCen).Sum(s => s.AvailableQuantity) <= 0)
        };
    }

    public async Task<IEnumerable<SellableProductResponseDto>> GetSellableProductsAsync(string companyCen, SellableProductQueryFilters? filters = null)
    {
        var products = await _productRepository.GetSellableByCompanyCenAsync(companyCen, filters);
        var result = new List<SellableProductResponseDto>();

        foreach (var product in products)
        {
            var stocks = await _stockRepository.GetByProductCenAsync(product.ProductCen);
            var availableQuantity = stocks.Sum(s => s.AvailableQuantity);

            result.Add(new SellableProductResponseDto
            {
                ProductCen = product.ProductCen,
                Name = product.Name,
                CategoryCen = product.CategoryCen,
                SalePrice = product.SalePrice,
                AvailableQuantity = availableQuantity,
                IsAvailable = availableQuantity > 0,
                StationCode = product.StationCode
            });
        }

        return result;
    }

    public async Task<StockValidationResponseDto> ValidateStockAsync(StockValidationRequestDto dto)
    {
        var requirements = new List<StockRequirementDto>();
        var isValid = true;

        foreach (var item in dto.Items)
        {
            var stock = await _stockRepository.GetAsync(item.ProductCen, dto.WarehouseCen);
            var available = stock?.AvailableQuantity ?? 0;

            if (available < item.Quantity)
            {
                isValid = false;
                requirements.Add(new StockRequirementDto
                {
                    ProductCen = item.ProductCen,
                    WarehouseCen = dto.WarehouseCen,
                    RequestedQuantity = item.Quantity,
                    AvailableQuantity = available,
                    MissingQuantity = item.Quantity - available,
                    Reason = "Insufficient stock"
                });
            }
        }

        return new StockValidationResponseDto
        {
            IsValid = isValid,
            Requirements = requirements
        };
    }

    private async Task<StockResponseDto> MapToDto(Stock entity)
    {
        var product = await _productRepository.GetByCenAsync(entity.ProductCen);
        var warehouse = await _warehouseRepository.GetByCenAsync(entity.WarehouseCen);

        string unitName = string.Empty;
        if (product != null && !string.IsNullOrEmpty(product.UnitCen))
        {
            var unit = await _unitRepository.GetByCenAsync(product.UnitCen);
            unitName = unit?.Name ?? string.Empty;
        }

        return new StockResponseDto
        {
            StockCen = entity.StockCen,
            ProductCen = entity.ProductCen,
            ProductName = product?.Name ?? "Unknown",
            WarehouseCen = entity.WarehouseCen,
            WarehouseName = warehouse?.Name ?? "Unknown",
            AvailableQuantity = entity.AvailableQuantity,
            ReservedQuantity = entity.ReservedQuantity,
            UnitName = unitName,
            ReorderLevel = product?.ReorderLevel ?? 0,
            IsLowStock = entity.AvailableQuantity <= (product?.ReorderLevel ?? 0)
        };
    }

    private static InventoryDocumentResponseDto MapToDto(InventoryDocument entity) => new()
    {
        DocumentCen = entity.DocumentCen,
        CompanyCen = entity.CompanyCen,
        WarehouseCen = entity.WarehouseCen,
        DocumentType = entity.DocumentType,
        Status = entity.Status,
        Title = entity.Title,
        Reason = entity.Reason,
        ExternalReference = entity.ExternalReference,
        CreatedAt = entity.CreatedAt,
        Lines = entity.Lines.Select(line => new DocumentLineResponseDto
        {
            LineCen = line.LineCen,
            ProductCen = line.ProductCen,
            Quantity = line.Quantity,
            UnitCost = line.UnitCost
        }).ToList()
    };

    private static KardexMovementResponseDto MapToDto(KardexMovement entity) => new()
    {
        MovementCen = entity.MovementCen,
        CompanyCen = entity.CompanyCen,
        WarehouseCen = entity.WarehouseCen,
        ProductCen = entity.ProductCen,
        DocumentCen = entity.DocumentCen,
        MovementType = entity.MovementType,
        Quantity = entity.Quantity,
        UnitCost = entity.UnitCost,
        Reason = entity.Reason,
        CreatedAt = entity.CreatedAt
    };
}
