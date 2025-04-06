namespace Redington.ProbabilityCalculator.Core.Requests;

public interface ICalculationInput
{
    double ProbabilityA { get; set; }
    double ProbabilityB { get; set; }
}