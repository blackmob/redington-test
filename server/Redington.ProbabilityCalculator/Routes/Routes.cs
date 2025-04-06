using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Redington.ProbabilityCalculator.Core.Filters;
using Redington.ProbabilityCalculator.Core.Models;
using Redington.ProbabilityCalculator.Core.Requests;

namespace Redington.ProbabilityCalculator.Routes;

public static class Routes
{
    public static void Map(WebApplication app) =>
        app.MapGet("/calculate/{calculationType}/{probabilityA:double}/{probabilityB:double}",
                async Task<Results<Ok<double>, BadRequest<Dictionary<string, string[]>>>>
                (IMediator mediator,
                    CalculationType calculationType,
                    double probabilityA,
                    double probabilityB) =>
                {
                    switch (calculationType)
                    {
                        case CalculationType.Either:
                            var eitherResult =
                                await mediator.Send(new EitherCalculationRequest(probabilityA,
                                    probabilityB));
                            return TypedResults.Ok(eitherResult.CalculatedProbability);
                        case CalculationType.CombinedWith:
                            var combinedWithResult = await mediator.Send(
                                new CombinedWithCalculationRequest(probabilityA, probabilityB));
                            return TypedResults.Ok(combinedWithResult.CalculatedProbability);
                        default:
                            return TypedResults.BadRequest(new Dictionary<string, string[]>
                            {
                                { "CalculationType", new[] { "Invalid calculation type" } }
                            });
                    }
                })
            .AddEndpointFilter<CalculationValidationFilter>()
            .WithName("CalculateProbability")
            .WithOpenApi();
}