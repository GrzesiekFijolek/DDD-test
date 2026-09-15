using Domain.Common.Exceptions;
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
        catch (Exception e)
        {
            await HandleException(e, context);
            
            _logger.LogWarning("An exception has occured: {ExceptionName}: {ExceptionMessage}", GetExceptionName(e), e.Message);
        }
    }

    private static async Task HandleException(Exception exception, HttpContext context)
    {
        var (statusCode, error) = exception switch
        {
            CustomException => (StatusCodes.Status400BadRequest, new Error(GetExceptionName(exception), exception.Message)),
            _ => (StatusCodes.Status500InternalServerError, new Error("error", "There was an error"))
        };

        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(error);
    }

    private static string GetExceptionName(Exception exception)
    {
        var exceptionName = exception.GetType()
            .Name.Replace("Exception", string.Empty)
            .Underscore();
        return exceptionName;
    }

    private record Error(string code, string reason);
}