using System.Diagnostics;
using Application.Common.CQRS;
using Humanizer;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Logging.Decorators;

internal sealed class LoggingComandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand  :  class, ICommand
{
    private readonly ICommandHandler<TCommand> _commandHandler;
    private readonly ILogger<ICommandHandler<TCommand>> _logger;

    public LoggingComandHandlerDecorator(ICommandHandler<TCommand> commandHandler, ILogger<ICommandHandler<TCommand>> logger)
    {
        _commandHandler = commandHandler;
        _logger = logger;
    }

    public async Task HandleAsync(TCommand command)
    {
        var commandName = typeof(TCommand).Name.Underscore();
        var stopwatch = new Stopwatch();
        
        stopwatch.Start();
        _logger.LogInformation("Started processing a command {CommandName}...", commandName);
        
        await _commandHandler.HandleAsync(command);
        
        stopwatch.Stop();
        _logger.LogInformation("Completed processing a command {CommandName} in {TimeElapsed}...", commandName, stopwatch.ElapsedMilliseconds);
        
    }
}

internal sealed class LoggingComandHandlerDecorator<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
    where TCommand : class, ICommand<TResponse> where TResponse : class
{
    private readonly ICommandHandler<TCommand, TResponse> _commandHandler;
    private readonly ILogger<ICommandHandler<TCommand, TResponse>> _logger;

    public LoggingComandHandlerDecorator(ICommandHandler<TCommand, TResponse> commandHandler, ILogger<ICommandHandler<TCommand, TResponse>> logger)
    {
        _commandHandler = commandHandler;
        _logger = logger;
    }

    public async Task<TResponse> HandleAsync(TCommand command)
    {
        var commandName = typeof(TCommand).Name.Underscore();
        var stopwatch = new Stopwatch();
        
        stopwatch.Start();
        _logger.LogInformation("Started processing a command {0}", commandName);
        
        var result = await _commandHandler.HandleAsync(command);
        
        stopwatch.Stop();
        _logger.LogInformation("Completed processing a command {0} in {1}...", commandName, stopwatch.ElapsedMilliseconds);

        return result;
    }
}