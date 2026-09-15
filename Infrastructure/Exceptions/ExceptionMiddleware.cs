using Domain.Common.Exceptions;
using FluentValidation;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Exceptions;

internal sealed class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException v)
        {
            var errors = GroupFailures(v);
            _logger.LogWarning("Validation failed: {ExceptionName}. Errors: {@ValidationErrors}",
                GetExceptionName(v), errors);

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new ValidationError("validation", errors));
        }
        catch (CustomException e)
        {
            _logger.LogError("An exception has occured: {ExceptionName}: {ExceptionMessage}",
                GetExceptionName(e), e.Message);

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new Error(GetExceptionName(e), e.Message));
        }
        catch (Exception e)
        {
            _logger.LogError("An unhandled exception has occured: {ExceptionName}: {ExceptionMessage}",
                GetExceptionName(e), e.Message);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new Error("error", "There was an error"));
        }
    }

    private static string GetExceptionName(Exception exception)
    {
        var exceptionName = exception.GetType()
            .Name.Replace("Exception", string.Empty)
            .Underscore();
        return exceptionName;
    }

    private static Dictionary<string, string[]> GroupFailures(ValidationException exception) =>
        exception.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

    private record Error(string code, string reason);

    private record ValidationError(string code, IReadOnlyDictionary<string, string[]> errors);
}