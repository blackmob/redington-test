using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Redington.ProbabilityCalculator.Core.OpenTelemetryExtensions;

[ExcludeFromCodeCoverage]
internal static class OpenTelemetryExtensions
{
    internal static IServiceCollection AddConfiguredOpenTelemetryInternal(this IServiceCollection services,
        IConfiguration configuration,
        ILoggingBuilder logging,
        string serviceName)
    {
        var serviceVersion = Assembly.GetCallingAssembly().GetName().Version?.ToString() ?? "unknown";

        var telemetrySection = configuration.GetSection(OpenTelemetryOptions.SectionKey);
        var telemetryConfig = telemetrySection.Get<OpenTelemetryOptions>() ?? new OpenTelemetryOptions();

        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(serviceName,
                serviceVersion: serviceVersion,
                serviceInstanceId: Environment.MachineName);

        services.AddLoggingInternal(resourceBuilder, logging, telemetryConfig, configuration);

        services.AddOpenTelemetry()
            .WithTracing(options =>
                AddTracingInternal(options, services, resourceBuilder, configuration, telemetryConfig))
            .WithMetrics(options =>
                AddMetricsInternal(options, services, resourceBuilder, telemetryConfig, configuration));

        return services;
    }

    private static void AddTracingInternal(TracerProviderBuilder options,
        IServiceCollection services,
        ResourceBuilder resourceBuilder,
        IConfiguration configuration,
        OpenTelemetryOptions telemetryConfig)
    {
        if (telemetryConfig.UseTracingExporter == OpenTelemetryOptions.Exporters.None)
            return;

        options.SetResourceBuilder(resourceBuilder)
            .SetSampler(new AlwaysOnSampler())
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation(o =>
            {
                o.EnrichWithHttpRequest = (activity, httpRequest) =>
                {
                    activity.SetTag("http.request.headers", httpRequest?.Headers.ToString());
                };
            });

        if (telemetryConfig.UseTracingExporter.HasFlag(OpenTelemetryOptions.Exporters.Console))
            options.AddConsoleExporter();

        if (telemetryConfig.UseTracingExporter.HasFlag(OpenTelemetryOptions.Exporters.AzureMonitor))
            options.AddAzureMonitorTraceExporter(x =>
                x.ConnectionString = telemetryConfig.AzureMonitor!.ConnectionString);

        if (telemetryConfig.UseTracingExporter.HasFlag(OpenTelemetryOptions.Exporters.Jaeger))
        {
            options.AddJaegerExporter();
            services.Configure<JaegerExporterOptions>(configuration.GetSection("TelemetryConfig").GetSection("Jaeger"));
        }

        if (telemetryConfig.UseTracingExporter.HasFlag(OpenTelemetryOptions.Exporters.Otlp))
            options.AddOtlpExporter(exporterOptions =>
            {
                exporterOptions.Endpoint = new Uri(configuration.GetSection("OpenTelemetryConfiguration")
                    .GetSection("Otlp").GetValue<string>("OTEL_EXPORTER_OTLP_ENDPOINT"));
                exporterOptions.TimeoutMilliseconds = configuration.GetSection("OpenTelemetryConfiguration")
                    .GetSection("Otlp").GetValue<int>("OTEL_ATTRIBUTE_VALUE_LENGTH_LIMIT");
            });

        if (!telemetryConfig.UseTracingExporter.HasFlag(OpenTelemetryOptions.Exporters.Console) &&
            !telemetryConfig.UseTracingExporter.HasFlag(OpenTelemetryOptions.Exporters.AzureMonitor) &&
            !telemetryConfig.UseTracingExporter.HasFlag(OpenTelemetryOptions.Exporters.Jaeger) &&
            !telemetryConfig.UseTracingExporter.HasFlag(OpenTelemetryOptions.Exporters.Otlp))
            options.AddConsoleExporter();

        // For options which can be bound from IConfiguration.
        var aspNetInstrumentation = configuration.GetSection("TelemetryConfig").GetSection("AspNetCoreInstrumentation");
        services.Configure<AspNetCoreTraceInstrumentationOptions>(aspNetInstrumentation);
    }

    private static void AddMetricsInternal(MeterProviderBuilder options,
        IServiceCollection _,
        ResourceBuilder resourceBuilder,
        OpenTelemetryOptions telemetryConfig,
        IConfiguration configuration)
    {
        if (telemetryConfig.UseMetricsExporter == OpenTelemetryOptions.Exporters.None)
            return;

        options.SetResourceBuilder(resourceBuilder)
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddRuntimeInstrumentation()
            .AddProcessInstrumentation()
            .AddEventCountersInstrumentation(x => x.AddEventSources("Microsoft.AspNetCore.Hosting"));

        if (telemetryConfig.UseMetricsExporter.HasFlag(OpenTelemetryOptions.Exporters.AzureMonitor))
            options.AddAzureMonitorMetricExporter(o =>
            {
                o.ConnectionString = telemetryConfig.AzureMonitor!.ConnectionString;
            });

        if (telemetryConfig.UseMetricsExporter.HasFlag(OpenTelemetryOptions.Exporters.Otlp))
            options.AddOtlpExporter(exporterOptions =>
            {
                exporterOptions.Endpoint = new Uri(configuration.GetSection("OpenTelemetryConfiguration")
                    .GetSection("Otlp").GetValue<string>("OTEL_EXPORTER_OTLP_ENDPOINT"));
                exporterOptions.TimeoutMilliseconds = configuration.GetSection("OpenTelemetryConfiguration")
                    .GetSection("Otlp").GetValue<int>("OTEL_ATTRIBUTE_VALUE_LENGTH_LIMIT");
            });

        if (!telemetryConfig.UseMetricsExporter.HasFlag(OpenTelemetryOptions.Exporters.AzureMonitor) &&
            !telemetryConfig.UseMetricsExporter.HasFlag(OpenTelemetryOptions.Exporters.Otlp))
            options.AddConsoleExporter();
    }

    private static IServiceCollection AddLoggingInternal(this IServiceCollection services,
        ResourceBuilder resourceBuilder,
        ILoggingBuilder loggingBuilder,
        OpenTelemetryOptions telemetryConfig,
        IConfiguration configuration)
    {
        if (telemetryConfig.UseLogExporter == OpenTelemetryOptions.Exporters.None) return services;

        loggingBuilder.AddOpenTelemetry(options =>
        {
            options.SetResourceBuilder(resourceBuilder);

            if (telemetryConfig.UseLogExporter.HasFlag(OpenTelemetryOptions.Exporters.AzureMonitor))
                options.AddAzureMonitorLogExporter(x =>
                    x.ConnectionString = telemetryConfig.AzureMonitor!.ConnectionString);

            if (telemetryConfig.UseLogExporter.HasFlag(OpenTelemetryOptions.Exporters.Otlp))
                options.AddOtlpExporter(exporterOptions =>
                {
                    exporterOptions.Endpoint = new Uri(configuration.GetSection("OpenTelemetryConfiguration")
                        .GetSection("Otlp").GetValue<string>("OTEL_EXPORTER_OTLP_ENDPOINT"));
                    exporterOptions.TimeoutMilliseconds = configuration.GetSection("OpenTelemetryConfiguration")
                        .GetSection("Otlp").GetValue<int>("OTEL_ATTRIBUTE_VALUE_LENGTH_LIMIT");
                });

            if (!telemetryConfig.UseLogExporter.HasFlag(OpenTelemetryOptions.Exporters.AzureMonitor) &&
                !telemetryConfig.UseLogExporter.HasFlag(OpenTelemetryOptions.Exporters.Otlp))
                options.AddConsoleExporter();
        });

        services.Configure<OpenTelemetryLoggerOptions>(opt =>
        {
            opt.IncludeScopes = true;
            opt.ParseStateValues = true;
            opt.IncludeFormattedMessage = true;
        });

        return services;
    }
}