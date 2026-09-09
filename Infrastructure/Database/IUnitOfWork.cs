namespace Infrastructure.Database;

public interface IUnitOfWork
{
    Task ExecuteAsync(Func<Task> action);
    Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action);
}