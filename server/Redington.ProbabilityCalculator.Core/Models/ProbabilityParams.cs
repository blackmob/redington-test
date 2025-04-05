using System.Diagnostics.CodeAnalysis;

namespace Redington.ProbabilityCalculator.Core.Models;

[ExcludeFromCodeCoverage]
public class ProbabilityParams : IProbabilityParams
{
    public ProbabilityParams(double probabilityA, double probabilityB)
    {
        ProbabilityA = probabilityA;
        ProbabilityB = probabilityB;
    }

    public double ProbabilityA { get; set; }
    public double ProbabilityB { get; set; }
}