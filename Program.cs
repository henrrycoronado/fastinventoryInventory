using DotNetEnv;

using Microsoft.EntityFrameworkCore;

using prismodInventory.Src.Application.Interfaces;
using prismodInventory.Src.Application.Services;
using prismodInventory.Src.Infraestructure.Persistence;
using prismodInventory.Src.Infraestructure.Persistence.Interfaces;
using prismodInventory.Src.Infraestructure.Persistence.Repositories;
using prismodInventory.Src.Presentation.MIddleware;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("La variable de entorno DB_CONNECTION_STRING no está configurada.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUnitRepository, UnitRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IInventoryDocumentRepository, InventoryDocumentRepository>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IKardexMovementRepository, KardexMovementRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUnitService, UnitService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
