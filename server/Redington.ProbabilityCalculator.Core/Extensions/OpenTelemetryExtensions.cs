using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Redington.ProbabilityCalculator.Core.OpenTelemetryExtensions;
using System.Diagnostics.CodeAnalysis;

namespace Redington.ProbabilityCalculator.Core.Extensions;

[ExcludeFromCodeCoverage]
public static class OpenTelemetryExtensions
{
    private const string _serviceNameSectionKey = "ServiceName";

    /// <summary>
    ///     Adds OpenTelemetry to the API.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> instance</param>
    /// <param name="configuration">The <see cref="IConfiguration" /> instance</param>
    /// <param name="loggingBuilder">The <see cref="ILoggingBuilder" /> instance</param>
    /// <param name="defaultServiceName">
    ///     The fallback service name if the configuration is blank. Preferred format is:
    ///     "CustomServiceName GQL"
    /// </param>
    /// <returns>The <see cref="IServiceCollection" /> for chaining more configurations</returns>
    public static IServiceCollection AddOpenTelemetryProviders(this IServiceCollection services,
        IConfiguration configuration,
        ILoggingBuilder loggingBuilder,
        string defaultServiceName)
    {
        var serviceName = configuration.GetValue<string>(_serviceNameSectionKey);

        serviceName ??= defaultServiceName;

        services.AddConfiguredOpenTelemetryInternal(configuration, loggingBuilder, serviceName);
        return services;
    }
}