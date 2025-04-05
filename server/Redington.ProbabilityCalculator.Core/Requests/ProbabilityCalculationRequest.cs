using Redington.ProbabilityCalculator.Core.Models;
using System.Diagnostics.CodeAnalysis;

namespace Redington.ProbabilityCalculator.Core.Requests;

[ExcludeFromCodeCoverage]
public record ProbabilityCalculationRequest(double ProbabilityA, double ProbabilityB, CalculationType CalculationType);