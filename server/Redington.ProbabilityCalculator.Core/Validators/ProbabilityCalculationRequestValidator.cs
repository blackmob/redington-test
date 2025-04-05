using FluentValidation;
using Redington.ProbabilityCalculator.Core.Requests;

namespace Redington.ProbabilityCalculator.Core.Validators;

public class ProbabilityCalculationRequestValidator : AbstractValidator<ProbabilityCalculationRequest>
{
    public ProbabilityCalculationRequestValidator()
    {
        RuleFor(x => x).NotNull().WithMessage("Request cannot be null");
        RuleFor(x => x.CalculationType)
            .NotNull()
            .WithMessage("Calculation type cannot be null")
            .IsInEnum()
            .WithMessage("Calculation type must be a valid enum value");
        RuleFor(x => x.ProbabilityA).InclusiveBetween(0, 1);
        RuleFor(x => x.ProbabilityB).InclusiveBetween(0, 1);
    }
}