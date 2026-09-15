using API.Endpoints.Auth;

namespace API.Endpoints;

public static class Extensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var api = app.MapGroup("/api");

        api.MapAuth();

        return app;
    }
}
