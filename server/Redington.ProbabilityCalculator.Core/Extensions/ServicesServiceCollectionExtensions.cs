using Microsoft.Extensions.DependencyInjection;
using Redington.ProbabilityCalculator.Core.Services;
using System.Diagnostics.CodeAnalysis;

namespace Redington.ProbabilityCalculator.Core.Extensions;

[ExcludeFromCodeCoverage]
public static class ServicesServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            var typesInAssembly = assembly.GetTypes();
            var serviceTypes = typesInAssembly.Where(x => x.IsAssignableTo(typeof(IService)) &&
                                                          x.IsInterface &&
                                                          x != typeof(IService));

            foreach (var interfaceType in serviceTypes)
            {
                var implementationTypes =
                    typesInAssembly.Where(x => x.IsAssignableTo(interfaceType) && x.IsClass);

                if (!implementationTypes.Any())
                {
                    throw new InvalidOperationException(
                        $"Found service interface '{interfaceType.Name}' with no implementation.");
                }

                foreach (var implementationType in implementationTypes)
                    services.AddTransient(interfaceType, implementationType);
            }
        }

        return services;
    }
}