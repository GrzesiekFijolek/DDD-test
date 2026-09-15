namespace API.OpenApi;

internal static class OpenApiExtensions
{
    public static IServiceCollection AddApiOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<OpenApiDocumentTransformer>();
        });

        return services;
    }

    public static WebApplication UseApiOpenApi(this WebApplication app)
    {
        app.MapOpenApi();

        app.UseReDoc(options =>
        {
            options.RoutePrefix = "docs";
            options.SpecUrl = "/openapi/v1.json";
            options.DocumentTitle = "FgProject API";
        });

        return app;
    }
}
