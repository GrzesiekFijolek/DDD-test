namespace Application.Common.CQRS;

public interface ISender
{
    Task SendAsync(ICommand command);

    Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command) where TResponse : class;

    Task<TResult> SendAsync<TResult>(IQuery<TResult> query);
}