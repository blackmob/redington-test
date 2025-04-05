using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Redington.ProbabilityCalculator.Core.Extensions;

[ExcludeFromCodeCoverage]
public static class ApiExtensions
{
    public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks();
        return services;
    }
}