using Application.Common.CQRS;

namespace Infrastructure.Database.Decorators;

internal sealed class UnitOfWorkCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand: class, ICommand
{
    private readonly ICommandHandler<TCommand> _commandHandler;
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkCommandHandlerDecorator(ICommandHandler<TCommand> commandHandler, IUnitOfWork unitOfWork)
    {
        _commandHandler = commandHandler;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(TCommand command)
    {
        await _unitOfWork.ExecuteAsync(() => _commandHandler.HandleAsync(command));
    }
}

internal sealed class UnitOfWorkCommandHandlerDecorator<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
    where TCommand : class, ICommand<TResponse> where TResponse : class
{
    private readonly ICommandHandler<TCommand, TResponse> _commandHandler;
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkCommandHandlerDecorator(ICommandHandler<TCommand, TResponse> commandHandler, IUnitOfWork unitOfWork)
    {
        _commandHandler = commandHandler;
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> HandleAsync(ICommand<TResponse> command)
    {
        return await _unitOfWork.ExecuteAsync(() => _commandHandler.HandleAsync(command));
    }
}