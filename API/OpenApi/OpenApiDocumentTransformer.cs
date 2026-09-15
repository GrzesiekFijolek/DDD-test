using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace API.OpenApi;

internal sealed class OpenApiDocumentTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        document.Info = new OpenApiInfo
        {
            Title = "FgProject API",
            Version = "v1",
            Description = "HTTP API for FgProject.",
        };

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Bearer token issued by the auth endpoint.",
        };

        document.Tags ??= new HashSet<OpenApiTag>();
        document.Tags.Add(new OpenApiTag
        {
            Name = "Auth",
            Description = "Sign-up, sign-in and token issuance for regular and admin users.",
        });

        return Task.CompletedTask;
    }
}
