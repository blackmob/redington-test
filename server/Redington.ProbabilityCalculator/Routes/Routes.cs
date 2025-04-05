using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Redington.ProbabilityCalculator.Core.Commands;
using Redington.ProbabilityCalculator.Core.Filters;
using Redington.ProbabilityCalculator.Core.Models;
using Redington.ProbabilityCalculator.Core.Requests;

namespace Redington.ProbabilityCalculator.Routes;

public static class Routes
{
    public static void Map(WebApplication app) =>
        app.MapPost("/calculate",
                async Task<Results<Ok<double>, BadRequest<Dictionary<string, string[]>>>>
                    (IMediator mediator, ProbabilityCalculationRequest request) =>
                {
                    switch (request.CalculationType)
                    {
                        case CalculationType.Either:
                            var eitherResult =
                                await mediator.Send(new EitherCalculationCommand(request.ProbabilityA,
                                    request.ProbabilityB));
                            return TypedResults.Ok(eitherResult.CalculatedProbability);
                        case CalculationType.CombinedWith:
                            var combinedWithResult = await mediator.Send(
                                new CombinedWithCalculationCommand(request.ProbabilityA, request.ProbabilityB));
                            return TypedResults.Ok(combinedWithResult.CalculatedProbability);
                        default:
                            return TypedResults.BadRequest(new Dictionary<string, string[]>
                            {
                                { "CalculationType", new[] { "Invalid calculation type" } }
                            });
                    }
                })
            .AddEndpointFilter<ValidationFilter<ProbabilityCalculationRequest>>()
            .WithName("CalculateProbability")
            .WithOpenApi();
}