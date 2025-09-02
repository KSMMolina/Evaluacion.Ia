using Api_test_ia.Application;
using Api_test_ia.Infrastructure;
using Api_test_ia.Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPresentation(builder.Configuration);

var app = builder.Build();

app.UsePresentation();

app.Run();