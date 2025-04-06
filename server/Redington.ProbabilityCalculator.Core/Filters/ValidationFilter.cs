using FluentValidation;
using Microsoft.AspNetCore.Http;
using Redington.ProbabilityCalculator.Core.Models;
using Redington.ProbabilityCalculator.Core.Requests;
using System.Diagnostics.CodeAnalysis;

namespace Redington.ProbabilityCalculator.Core.Filters;

[ExcludeFromCodeCoverage]
public class CalculationValidationFilter : IEndpointFilter
{
    private readonly IValidator<ProbabilityCalculationRequest> _validator;

    public CalculationValidationFilter(IValidator<ProbabilityCalculationRequest> validator)
    {
        _validator = validator;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var calculationType = context.Arguments.OfType<CalculationType>().SingleOrDefault();
        var probabilities = context.Arguments.OfType<double>();

        var result = await _validator.ValidateAsync(
            new ProbabilityCalculationRequest(probabilities.FirstOrDefault(), probabilities.LastOrDefault(), calculationType),
            context.HttpContext.RequestAborted);

        if (!result.IsValid) return TypedResults.ValidationProblem(result.ToDictionary());
        return await next(context);
    }
}