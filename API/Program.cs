using API;
using API.Endpoints;
using API.OpenApi;
using Application;
using Domain;
using Infrastructure;

DotNetEnv.Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiOpenApi();

builder.Services.AddDomain()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.UseSerilogLogging();

var app = builder.Build();

app.UseInfrastructure();

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseApiOpenApi();
}

app.Run();
