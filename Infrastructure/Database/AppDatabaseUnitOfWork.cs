using Application.Security;
using Domain.Common;
using Domain.Common.Audit;
using Domain.Common.Exceptions;
using Infrastructure.Database.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

internal sealed class AppDatabaseUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _appDbContext;
    private readonly IClock _clock;
    private readonly ICurrentUser _currentUser;

    public AppDatabaseUnitOfWork(AppDbContext appDbContext, IClock clock, ICurrentUser currentUser)
    {
        _appDbContext = appDbContext;
        _clock = clock;
        _currentUser = currentUser;
    }

    public async Task ExecuteAsync(Func<Task> action)
    {
        await using var transaction = await _appDbContext.Database.BeginTransactionAsync();

        try
        {
            await action();
            StampAuditFields();
            await _appDbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action)
    {
        await using var transaction = await _appDbContext.Database.BeginTransactionAsync();

        try
        {
            var result = await action();
            StampAuditFields();
            await _appDbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return result;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            Console.WriteLine(e);
            throw;
        }
    }

    private void StampAuditFields()
    {
        var now = _clock.Current();
        var creationUserId = _currentUser.IsAuthenticated ? _currentUser.UserId : null;
        var creationUserName = _currentUser.IsAuthenticated ? _currentUser.UserName : null;

        foreach (var entry in _appDbContext.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity is ICreationAuditableEntity creatable)
                        creatable.CreationInfo = new CreationAudit(now, creationUserId, creationUserName);
                    break;

                case EntityState.Modified:
                    if (entry.Entity is IModificationAuditableEntity modifiable)
                    {
                        var (id, name) = RequireAuthenticatedUser("modify");
                        modifiable.ModificationInfo = new ModificationAudit(now, id, name);
                    }
                    break;

                case EntityState.Deleted:
                    if (entry.Entity is ISoftDeleteAuditableEntity deletable)
                    {
                        var (id, name) = RequireAuthenticatedUser("delete");
                        entry.State = EntityState.Modified;
                        deletable.DeletionInfo = new DeletionAudit(now, id, name);
                    }
                    break;
            }
        }
    }

    private (long UserId, string UserName) RequireAuthenticatedUser(string action)
    {
        if (!_currentUser.IsAuthenticated || _currentUser.UserId is null || _currentUser.UserName is null)
            throw new UnauthenticatedAuditActionException(action);

        return (_currentUser.UserId.Value, _currentUser.UserName);
    }
}
