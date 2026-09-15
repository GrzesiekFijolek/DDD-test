using Application.Auth.Commands;
using Application.Auth.Requests;
using Application.Common.CQRS;
using Application.Common.Errors;

namespace API.Endpoints.Auth;

internal static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuth(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth").WithTags("Auth");

        group.MapPost("/sign-up", async (RegularUserSignUpRequest request, ISender sender) =>
            {
                await sender.SendAsync(new RegularUserSignUpCommand(request));
                return Results.NoContent();
            })
            .WithName("SignUp")
            .WithSummary("Register a new regular user")
            .WithDescription("Creates a new regular user account with the given email, username, password and department.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ValidationErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status422UnprocessableEntity)
            .AllowAnonymous();

        return group;
    }
}
