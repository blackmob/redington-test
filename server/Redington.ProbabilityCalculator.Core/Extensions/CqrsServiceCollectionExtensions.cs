using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Redington.ProbabilityCalculator.Core.Extensions;

[ExcludeFromCodeCoverage]
public static class CqrsServiceCollectionExtensions
{
    public static IServiceCollection AddCqrsMediatRServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        return services;
    }
}