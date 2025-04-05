using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Redington.ProbabilityCalculator.Core.Extensions;
using Redington.ProbabilityCalculator.Routes;
using System.Diagnostics.CodeAnalysis;

[assembly: ExcludeFromCodeCoverage]

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddProblemDetails()
    .AddHttpContextAccessor()
    .AddCors()
    .AddResponseCompression()
    .AddCustomHealthChecks()
    .AddCqrsMediatRServices()
    .AddServices()
    .AddCoreServices(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseResponseCompression();

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResultStatusCodes =
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status200OK,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
        }
    });

Routes.Map(app);

app.Run();