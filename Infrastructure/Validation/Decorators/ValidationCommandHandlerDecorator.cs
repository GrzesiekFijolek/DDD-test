using Application.Common.CQRS;
using FluentValidation;
using FluentValidation.Results;

namespace Infrastructure.Validation.Decorators;

internal sealed class ValidationCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : class, ICommand
{
    private readonly ICommandHandler<TCommand> _commandHandler;
    private readonly IEnumerable<IValidator<TCommand>> _validators;

    public ValidationCommandHandlerDecorator(ICommandHandler<TCommand> commandHandler, IEnumerable<IValidator<TCommand>> validators)
    {
        _commandHandler = commandHandler;
        _validators = validators;
    }

    public async Task HandleAsync(TCommand command)
    {
        var failures = new List<ValidationFailure>();

        foreach (var validator in _validators)
        {
            var result = await validator.ValidateAsync(command);
            if (!result.IsValid)
                failures.AddRange(result.Errors);
        }

        if (failures.Count > 0)
            throw new ValidationException(failures);

        await _commandHandler.HandleAsync(command);
    }
}

internal sealed class ValidationCommandHandlerDecorator<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
    where TCommand : class, ICommand<TResponse> where TResponse : class
{
    private readonly ICommandHandler<TCommand, TResponse> _commandHandler;
    private readonly IEnumerable<IValidator<TCommand>> _validators;

    public ValidationCommandHandlerDecorator(ICommandHandler<TCommand, TResponse> commandHandler, IEnumerable<IValidator<TCommand>> validators)
    {
        _commandHandler = commandHandler;
        _validators = validators;
    }

    public async Task<TResponse> HandleAsync(TCommand command)
    {
        var failures = new List<ValidationFailure>();

        foreach (var validator in _validators)
        {
            var result = await validator.ValidateAsync(command);
            if (!result.IsValid)
                failures.AddRange(result.Errors);
        }

        if (failures.Count > 0)
            throw new ValidationException(failures);

        return await _commandHandler.HandleAsync(command);
    }
}
