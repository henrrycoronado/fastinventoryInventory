using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using prismodInventory.Src.Infraestructure.Persistence.Interfaces;

namespace prismodInventory.Src.Infraestructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private IDbContextTransaction? _currentTransaction;

    public UnitOfWork(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await _dbContext.SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction == null) _currentTransaction = await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try { await SaveChangesAsync(); if (_currentTransaction != null) await _currentTransaction.CommitAsync(); }
        finally { if (_currentTransaction != null) { await _currentTransaction.DisposeAsync(); _currentTransaction = null; } }
    }

    public async Task RollbackTransactionAsync()
    {
        try { if (_currentTransaction != null) await _currentTransaction.RollbackAsync(); }
        finally { if (_currentTransaction != null) { await _currentTransaction.DisposeAsync(); _currentTransaction = null; } }
    }
}
