using FluentValidation;
using Redington.ProbabilityCalculator.Core.Models;
using System.Diagnostics.CodeAnalysis;

namespace Redington.ProbabilityCalculator.Core.Services;

public class EitherCalculatorService : IEitherCalculatorService
{
    private readonly IValidator<IProbabilityParams> _validator;

    public EitherCalculatorService(IValidator<IProbabilityParams> validator)
    {
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    [ExcludeFromCodeCoverage]
    public async ValueTask DisposeAsync()
    {
        await ValueTask.CompletedTask;
        GC.SuppressFinalize(this);
    }

    public async Task<double> CalculateProbabilityAsync(IProbabilityParams probabilityParams)
    {
        var validationResult = await _validator.ValidateAsync(probabilityParams);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        return probabilityParams.ProbabilityA + probabilityParams.ProbabilityB -
               (probabilityParams.ProbabilityA * probabilityParams.ProbabilityB);
    }
}