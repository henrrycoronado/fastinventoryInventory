using fastinventoryInventory.Src.Application.DTOs.Common;
using fastinventoryInventory.Src.Application.DTOs.Products;
using fastinventoryInventory.Src.Application.Interfaces;
using fastinventoryInventory.Src.Domain.Entities;
using fastinventoryInventory.Src.Infraestructure.Persistence.Interfaces;

namespace fastinventoryInventory.Src.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitRepository _unitRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(
        IProductRepository repository,
        ICategoryRepository categoryRepository,
        IUnitRepository unitRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _unitRepository = unitRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductResponseDto?> GetByCenAsync(string productCen)
    {
        var entity = await _repository.GetByCenAsync(productCen);
        return entity == null ? null : await MapToDto(entity);
    }

    public async Task<ProductResponseDto?> GetBySkuAsync(string companyCen, string sku)
    {
        var entity = await _repository.GetBySkuAsync(companyCen, sku);
        return entity == null ? null : await MapToDto(entity);
    }

    public async Task<IEnumerable<ProductResponseDto>> GetByCompanyAsync(string companyCen, ProductQueryFilters? filters = null)
    {
        var entities = await _repository.GetByCompanyCenAsync(companyCen, filters);
        var dtos = new List<ProductResponseDto>();
        foreach (var entity in entities)
        {
            dtos.Add(await MapToDto(entity));
        }
        return dtos;
    }

    public async Task<IEnumerable<ProductResponseDto>> GetByCensAsync(IEnumerable<string> productCens)
    {
        var result = new List<ProductResponseDto>();
        foreach (var cen in productCens)
        {
            var entity = await _repository.GetByCenAsync(cen);
            if (entity != null)
            {
                result.Add(await MapToDto(entity));
            }
        }
        return result;
    }

    public async Task<CreateProductResponseDto> CreateAsync(CreateProductDto dto)
    {
        var entity = new Product(dto.CompanyCen, dto.Sku, dto.Name, dto.SalePrice, dto.CostPrice, dto.ReorderLevel);

        if (!string.IsNullOrWhiteSpace(dto.CategoryCen))
        {
            entity.AssignCategory(dto.CategoryCen);
        }

        if (!string.IsNullOrWhiteSpace(dto.UnitCen))
        {
            entity.AssignUnit(dto.UnitCen);
        }

        if (!string.IsNullOrWhiteSpace(dto.Description))
        {
            typeof(Product).GetProperty(nameof(Product.Description))?.SetValue(entity, dto.Description);
        }

        typeof(Product).GetProperty(nameof(Product.StationCode))?.SetValue(entity, dto.StationCode);

        await _repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return new CreateProductResponseDto
        {
            ProductCen = entity.ProductCen,
            Sku = entity.Sku,
            Name = entity.Name,
            Status = entity.Status,
            InitialStock = 0
        };
    }

    public async Task UpdateAsync(string productCen, UpdateProductDto dto)
    {
        var entity = await _repository.GetByCenAsync(productCen);
        if (entity == null)
        {
            throw new InvalidOperationException("Product not found");
        }

        typeof(Product).GetProperty(nameof(Product.ProductCen))?.SetValue(entity, productCen);
        typeof(Product).GetProperty(nameof(Product.Sku))?.SetValue(entity, dto.Sku);
        typeof(Product).GetProperty(nameof(Product.Name))?.SetValue(entity, dto.Name);
        typeof(Product).GetProperty(nameof(Product.Description))?.SetValue(entity, dto.Description);
        typeof(Product).GetProperty(nameof(Product.ReorderLevel))?.SetValue(entity, dto.ReorderLevel);

        entity.UpdatePrices(dto.SalePrice, dto.CostPrice);

        if (!string.IsNullOrWhiteSpace(dto.CategoryCen))
        {
            entity.AssignCategory(dto.CategoryCen);
        }
        else
        {
            typeof(Product).GetProperty(nameof(Product.CategoryCen))?.SetValue(entity, null);
        }

        if (!string.IsNullOrWhiteSpace(dto.UnitCen))
        {
            entity.AssignUnit(dto.UnitCen);
        }
        else
        {
            typeof(Product).GetProperty(nameof(Product.UnitCen))?.SetValue(entity, null);
        }

        entity.ChangeStatus(dto.Status);
        typeof(Product).GetProperty(nameof(Product.StationCode))?.SetValue(entity, dto.StationCode);

        await _repository.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateStatusAsync(string productCen, UpdateProductStatusDto dto)
    {
        var entity = await _repository.GetByCenAsync(productCen);
        if (entity == null)
        {
            throw new InvalidOperationException("Product not found");
        }

        entity.ChangeStatus(dto.Status);

        await _repository.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task<ProductResponseDto> MapToDto(Product entity)
    {
        string? categoryName = null;
        if (!string.IsNullOrEmpty(entity.CategoryCen))
        {
            var cat = await _categoryRepository.GetByCenAsync(entity.CategoryCen);
            categoryName = cat?.Name;
        }

        string? unitName = null;
        if (!string.IsNullOrEmpty(entity.UnitCen))
        {
            var unit = await _unitRepository.GetByCenAsync(entity.UnitCen);
            unitName = unit?.Name;
        }

        return new ProductResponseDto
        {
            ProductCen = entity.ProductCen,
            CompanyCen = entity.CompanyCen,
            CategoryCen = entity.CategoryCen,
            CategoryName = categoryName,
            UnitCen = entity.UnitCen,
            UnitName = unitName,
            Sku = entity.Sku,
            Name = entity.Name,
            Description = entity.Description,
            SalePrice = entity.SalePrice,
            CostPrice = entity.CostPrice,
            ReorderLevel = entity.ReorderLevel,
            Status = entity.Status,
            StationCode = entity.StationCode
        };
    }
}
