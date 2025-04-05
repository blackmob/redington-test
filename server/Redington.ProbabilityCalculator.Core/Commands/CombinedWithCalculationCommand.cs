using MediatR;
using Redington.ProbabilityCalculator.Core.Payloads;
using System.Diagnostics.CodeAnalysis;

namespace Redington.ProbabilityCalculator.Core.Commands;

[ExcludeFromCodeCoverage]
public class CombinedWithCalculationCommand : IRequest<CalculationResultPayload>, ICalculationInput
{
    public CombinedWithCalculationCommand(double probabilityA, double probabilityB)
    {
        ProbabilityA = probabilityA;
        ProbabilityB = probabilityB;
    }

    public double ProbabilityA { get; set; }
    public double ProbabilityB { get; set; }
}