namespace Application.Common.CQRS;

public interface ICommandHandler<in TCommand> where TCommand: ICommand
{
    Task HandleAsync(TCommand command);
}

public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand<TResponse> where TResponse : class
{
    Task<TResponse> HandleAsync(TCommand command);
}