using FluentValidation;
using Redington.ProbabilityCalculator.Core.Models;

namespace Redington.ProbabilityCalculator.Core.Validators;

public class ProbabilityParamsValidator : AbstractValidator<IProbabilityParams>
{
    public ProbabilityParamsValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .WithMessage("Probability parameters cannot be null.");

        RuleFor(x => x.ProbabilityA)
            .InclusiveBetween(0, 1)
            .WithMessage("Probability A must be between 0 and 1.");

        RuleFor(x => x.ProbabilityB)
            .InclusiveBetween(0, 1)
            .WithMessage("Probability B must be between 0 and 1.");
    }
}