using Domain.Common.Exceptions;
using Humanizer;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Exceptions;

internal sealed class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            await HandleException(e, context);
        }
    }

    private static async Task HandleException(Exception exception, HttpContext context)
    {
        var (statusCode, error) = exception switch
        {
            CustomException => (StatusCodes.Status400BadRequest, new Error(exception.GetType()
                .Name.Replace("Exception", string.Empty).Underscore(), exception.Message)),
            _ => (StatusCodes.Status500InternalServerError, new Error("error", "There was an error"))
        };

        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(error);
    }

    private record Error(string code, string reason);
}