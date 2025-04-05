using MediatR;
using Redington.ProbabilityCalculator.Core.Payloads;
using System.Diagnostics.CodeAnalysis;

namespace Redington.ProbabilityCalculator.Core.Commands;

[ExcludeFromCodeCoverage]
public class EitherCalculationCommand : IRequest<CalculationResultPayload>, ICalculationInput
{
    public EitherCalculationCommand(double probabilityA, double probabilityB)
    {
        ProbabilityA = probabilityA;
        ProbabilityB = probabilityB;
    }

    public double ProbabilityA { get; set; }
    public double ProbabilityB { get; set; }
}