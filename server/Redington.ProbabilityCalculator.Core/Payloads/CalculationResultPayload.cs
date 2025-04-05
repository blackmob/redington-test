using System.Diagnostics.CodeAnalysis;

namespace Redington.ProbabilityCalculator.Core.Payloads;

[ExcludeFromCodeCoverage]
public record CalculationResultPayload(double CalculatedProbability);