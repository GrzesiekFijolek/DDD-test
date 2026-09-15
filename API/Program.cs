using API;
using Application;
using Domain;
using Infrastructure;

DotNetEnv.Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDomain()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.UseSerilogLogging();

var app = builder.Build();

app.UseInfrastructure();

app.MapEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Run();
