using fastinventoryInventory.Src.Infraestructure.Persistence.Models;

using Microsoft.EntityFrameworkCore;

namespace fastinventoryInventory.Src.Infraestructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<CompanyModel> Companies { get; set; } = null!;
    public DbSet<WarehouseModel> Warehouses { get; set; } = null!;
    public DbSet<CategoryModel> Categories { get; set; } = null!;
    public DbSet<UnitModel> Units { get; set; } = null!;
    public DbSet<ProductModel> Products { get; set; } = null!;
    public DbSet<StockModel> Stocks { get; set; } = null!;
    public DbSet<InventoryDocumentModel> InventoryDocuments { get; set; } = null!;
    public DbSet<InventoryDocumentLineModel> InventoryDocumentLines { get; set; } = null!;
    public DbSet<KardexMovementModel> KardexMovements { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("inventory");

        modelBuilder.Entity<CompanyModel>(e =>
        {
            e.ToTable("companies");
            e.HasKey(x => x.Id);
            e.HasAlternateKey(x => x.CompanyCen);

            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.CompanyCen).HasColumnName("company_cen");
            e.Property(x => x.Name).HasColumnName("name");
            e.Property(x => x.IsActive).HasColumnName("is_active");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<WarehouseModel>(e =>
        {
            e.ToTable("warehouses");
            e.HasKey(x => x.Id);
            e.HasAlternateKey(x => x.WarehouseCen);

            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.WarehouseCen).HasColumnName("warehouse_cen");
            e.Property(x => x.CompanyCen).HasColumnName("company_cen");
            e.Property(x => x.Name).HasColumnName("name");
            e.Property(x => x.IsActive).HasColumnName("is_active");

            e.HasOne(w => w.Company)
                .WithMany(c => c.Warehouses)
                .HasForeignKey(w => w.CompanyCen)
                .HasPrincipalKey(c => c.CompanyCen);
        });

        modelBuilder.Entity<CategoryModel>(e =>
        {
            e.ToTable("categories");
            e.HasKey(x => x.Id);
            e.HasAlternateKey(x => x.CategoryCen);

            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.CategoryCen).HasColumnName("category_cen");
            e.Property(x => x.CompanyCen).HasColumnName("company_cen");
            e.Property(x => x.Name).HasColumnName("name");
            e.Property(x => x.Description).HasColumnName("description");
            e.Property(x => x.IsActive).HasColumnName("is_active");

            e.HasOne(c => c.Company)
                .WithMany(comp => comp.Categories)
                .HasForeignKey(c => c.CompanyCen)
                .HasPrincipalKey(comp => comp.CompanyCen);
        });

        modelBuilder.Entity<UnitModel>(e =>
        {
            e.ToTable("units");
            e.HasKey(x => x.Id);
            e.HasAlternateKey(x => x.UnitCen);

            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.UnitCen).HasColumnName("unit_cen");
            e.Property(x => x.CompanyCen).HasColumnName("company_cen");
            e.Property(x => x.Name).HasColumnName("name");
            e.Property(x => x.Abbreviation).HasColumnName("abbreviation");
            e.Property(x => x.IsActive).HasColumnName("is_active");

            e.HasOne(u => u.Company)
                .WithMany(c => c.Units)
                .HasForeignKey(u => u.CompanyCen)
                .HasPrincipalKey(c => c.CompanyCen);
        });

        modelBuilder.Entity<ProductModel>(e =>
        {
            e.ToTable("products");
            e.HasKey(x => x.Id);
            e.HasAlternateKey(x => x.ProductCen);

            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.ProductCen).HasColumnName("product_cen");
            e.Property(x => x.CompanyCen).HasColumnName("company_cen");
            e.Property(x => x.CategoryCen).HasColumnName("category_cen");
            e.Property(x => x.UnitCen).HasColumnName("unit_cen");
            e.Property(x => x.Sku).HasColumnName("sku");
            e.Property(x => x.Name).HasColumnName("name");
            e.Property(x => x.Description).HasColumnName("description");
            e.Property(x => x.SalePrice).HasColumnName("sale_price");
            e.Property(x => x.CostPrice).HasColumnName("cost_price");
            e.Property(x => x.ReorderLevel).HasColumnName("reorder_level");
            e.Property(x => x.Status).HasColumnName("status");
            e.Property(x => x.StationCode).HasColumnName("station_code");

            e.HasOne(p => p.Company).WithMany(c => c.Products).HasForeignKey(p => p.CompanyCen).HasPrincipalKey(c => c.CompanyCen);
            e.HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryCen).HasPrincipalKey(c => c.CategoryCen);
            e.HasOne(p => p.Unit).WithMany(u => u.Products).HasForeignKey(p => p.UnitCen).HasPrincipalKey(u => u.UnitCen);
        });

        modelBuilder.Entity<StockModel>(e =>
        {
            e.ToTable("stock");
            e.HasKey(x => x.Id);
            e.HasAlternateKey(x => x.StockCen);

            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.StockCen).HasColumnName("stock_cen");
            e.Property(x => x.ProductCen).HasColumnName("product_cen");
            e.Property(x => x.WarehouseCen).HasColumnName("warehouse_cen");
            e.Property(x => x.AvailableQuantity).HasColumnName("available_quantity");
            e.Property(x => x.ReservedQuantity).HasColumnName("reserved_quantity");

            e.HasOne(s => s.Product).WithMany(p => p.Stocks).HasForeignKey(s => s.ProductCen).HasPrincipalKey(p => p.ProductCen);
            e.HasOne(s => s.Warehouse).WithMany(w => w.Stocks).HasForeignKey(s => s.WarehouseCen).HasPrincipalKey(w => w.WarehouseCen);
        });

        modelBuilder.Entity<InventoryDocumentModel>(e =>
        {
            e.ToTable("inventory_documents");
            e.HasKey(x => x.Id);
            e.HasAlternateKey(x => x.DocumentCen);

            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.DocumentCen).HasColumnName("document_cen");
            e.Property(x => x.CompanyCen).HasColumnName("company_cen");
            e.Property(x => x.WarehouseCen).HasColumnName("warehouse_cen");
            e.Property(x => x.DocumentType).HasColumnName("document_type");
            e.Property(x => x.Status).HasColumnName("status");
            e.Property(x => x.Title).HasColumnName("title");
            e.Property(x => x.Reason).HasColumnName("reason");
            e.Property(x => x.ExternalReference).HasColumnName("external_reference");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");

            e.HasOne(d => d.Company).WithMany().HasForeignKey(d => d.CompanyCen).HasPrincipalKey(c => c.CompanyCen);
            e.HasOne(d => d.Warehouse).WithMany().HasForeignKey(d => d.WarehouseCen).HasPrincipalKey(w => w.WarehouseCen);
        });

        modelBuilder.Entity<InventoryDocumentLineModel>(e =>
        {
            e.ToTable("inventory_document_lines");
            e.HasKey(x => x.Id);
            e.HasAlternateKey(x => x.LineCen);

            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.LineCen).HasColumnName("line_cen");
            e.Property(x => x.DocumentCen).HasColumnName("document_cen");
            e.Property(x => x.ProductCen).HasColumnName("product_cen");
            e.Property(x => x.Quantity).HasColumnName("quantity");
            e.Property(x => x.UnitCost).HasColumnName("unit_cost");

            e.HasOne(l => l.Document).WithMany(d => d.Lines).HasForeignKey(l => l.DocumentCen).HasPrincipalKey(d => d.DocumentCen);
            e.HasOne(l => l.Product).WithMany().HasForeignKey(l => l.ProductCen).HasPrincipalKey(p => p.ProductCen);
        });

        modelBuilder.Entity<KardexMovementModel>(e =>
        {
            e.ToTable("kardex_movements");
            e.HasKey(x => x.Id);
            e.HasAlternateKey(x => x.MovementCen);

            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.MovementCen).HasColumnName("movement_cen");
            e.Property(x => x.CompanyCen).HasColumnName("company_cen");
            e.Property(x => x.WarehouseCen).HasColumnName("warehouse_cen");
            e.Property(x => x.ProductCen).HasColumnName("product_cen");
            e.Property(x => x.DocumentCen).HasColumnName("document_cen");
            e.Property(x => x.MovementType).HasColumnName("movement_type");
            e.Property(x => x.Quantity).HasColumnName("quantity");
            e.Property(x => x.UnitCost).HasColumnName("unit_cost");
            e.Property(x => x.Reason).HasColumnName("reason");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");

            e.HasOne(k => k.Company).WithMany().HasForeignKey(k => k.CompanyCen).HasPrincipalKey(c => c.CompanyCen);
            e.HasOne(k => k.Warehouse).WithMany().HasForeignKey(k => k.WarehouseCen).HasPrincipalKey(w => w.WarehouseCen);
            e.HasOne(k => k.Product).WithMany().HasForeignKey(k => k.ProductCen).HasPrincipalKey(p => p.ProductCen);
            e.HasOne(k => k.Document).WithMany().HasForeignKey(k => k.DocumentCen).HasPrincipalKey(d => d.DocumentCen);
        });
    }
}
