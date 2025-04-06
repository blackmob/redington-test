using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.AspNetCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Redington.ProbabilityCalculator.Core.OpenTelemetryExtensions;

/// <summary>
///     OpenTelemetry configuration options.
/// </summary>
[ExcludeFromCodeCoverage]
public class OpenTelemetryOptions
{
    [Flags]
    public enum Exporters
    {
        None = 0,
        Console = 1,
        AzureMonitor = 2,
        Jaeger = 4,
        Otlp = 8
    }

    public const string SectionKey = "OpenTelemetryConfiguration";

    [Required] public Exporters UseTracingExporter { get; set; } = Exporters.Console;

    [Required] public Exporters UseMetricsExporter { get; set; } = Exporters.Console;

    [Required] public Exporters UseLogExporter { get; set; } = Exporters.Console;

    public JaegerExporterOptions? Jaeger { get; set; }
    public AspNetCoreTraceInstrumentationOptions? AspNetCoreInstrumentation { get; set; }
    public AzureMonitorOptions? AzureMonitor { get; set; }

    public OtlpExporterOptions? Otlp { get; set; }

    public sealed class AzureMonitorOptions
    {
        [Required] public string ConnectionString { get; set; } = default!;
    }
}