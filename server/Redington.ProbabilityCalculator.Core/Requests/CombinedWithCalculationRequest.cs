using MediatR;
using Redington.ProbabilityCalculator.Core.Payloads;

namespace Redington.ProbabilityCalculator.Core.Requests;

public class CombinedWithCalculationRequest : IRequest<CalculationResultPayload>, ICalculationInput
{
    public CombinedWithCalculationRequest(double probabilityA, double probabilityB)
    {
        ProbabilityA = probabilityA;
        ProbabilityB = probabilityB;
        
    }

    public double ProbabilityA { get; set; }
    public double ProbabilityB { get; set; }
}