using MediatR;
using Microsoft.Extensions.Logging;
using Redington.ProbabilityCalculator.Core.Models;
using Redington.ProbabilityCalculator.Core.Payloads;
using Redington.ProbabilityCalculator.Core.Requests;
using Redington.ProbabilityCalculator.Core.Services;

namespace Redington.ProbabilityCalculator.Core.Handlers;

public class EitherCalculationHandler : IRequestHandler<EitherCalculationRequest, CalculationResultPayload>
{
    private readonly ILogger<EitherCalculationHandler> _logger;
    private readonly IEitherCalculatorService _service;

    public EitherCalculationHandler(ILogger<EitherCalculationHandler> logger, IEitherCalculatorService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<CalculationResultPayload> Handle(EitherCalculationRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Calculating probability for A: {ProbabilityA} and B: {ProbabilityB} using the Either method",
            request.ProbabilityA, request.ProbabilityB);

        var result =
            await _service.CalculateProbabilityAsync(new ProbabilityParams(request.ProbabilityA,
                request.ProbabilityB));

        _logger.LogInformation("Calculation result: {Result} for the Either method", result);

        return await Task.FromResult(new CalculationResultPayload(result));
    }
}