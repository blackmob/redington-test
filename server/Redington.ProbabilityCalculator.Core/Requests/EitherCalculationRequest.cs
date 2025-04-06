using MediatR;
using Redington.ProbabilityCalculator.Core.Payloads;

namespace Redington.ProbabilityCalculator.Core.Requests;

public class EitherCalculationRequest : IRequest<CalculationResultPayload>, ICalculationInput
{
    public EitherCalculationRequest(double probabilityA, double probabilityB)
    {
        ProbabilityA = probabilityA;
        ProbabilityB = probabilityB;
        
    }

    public double ProbabilityA { get; set; }
    public double ProbabilityB { get; set; }
}