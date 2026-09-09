namespace Infrastructure.Database;

internal sealed class AppDatabaseUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _appDbContext;

    public AppDatabaseUnitOfWork(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task ExecuteAsync(Func<Task> action)
    {
        await using var transaction = await _appDbContext.Database.BeginTransactionAsync();

        try
        {
            await action();
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
}