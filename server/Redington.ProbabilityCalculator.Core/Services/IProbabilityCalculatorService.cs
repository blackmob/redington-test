using Redington.ProbabilityCalculator.Core.Models;

namespace Redington.ProbabilityCalculator.Core.Services;

/// <summary>
///     Interface for a calculator service that provides probability calculations.
/// </summary>
public interface IProbabilityCalculatorService : IService
{
    /// <summary>
    ///     Calculates the probability.
    /// </summary>
    /// <param name="probabilityParams">Contains the probability parameters</param>
    /// <returns>The Task<double> that contains the calculated probability.</returns>
    Task<double> CalculateProbabilityAsync(IProbabilityParams probabilityParams);
}