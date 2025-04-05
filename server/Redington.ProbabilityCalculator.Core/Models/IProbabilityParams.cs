namespace Redington.ProbabilityCalculator.Core.Models;

public interface IProbabilityParams
{
    /// <summary>
    ///     Gets or sets the probability of event A.
    /// </summary>
    double ProbabilityA { get; set; }

    /// <summary>
    ///     Gets or sets the probability of event B.
    /// </summary>
    double ProbabilityB { get; set; }
}