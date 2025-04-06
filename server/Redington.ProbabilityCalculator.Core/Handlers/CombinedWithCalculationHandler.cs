using MediatR;
using Microsoft.Extensions.Logging;
using Redington.ProbabilityCalculator.Core.Models;
using Redington.ProbabilityCalculator.Core.Payloads;
using Redington.ProbabilityCalculator.Core.Requests;
using Redington.ProbabilityCalculator.Core.Services;

namespace Redington.ProbabilityCalculator.Core.Handlers;

public class CombinedWithCalculationHandler : IRequestHandler<CombinedWithCalculationRequest, CalculationResultPayload>
{
    private readonly ILogger<CombinedWithCalculationHandler> _logger;
    private readonly ICombinedWithCalculatorService _service;

    public CombinedWithCalculationHandler(ILogger<CombinedWithCalculationHandler> logger,
        ICombinedWithCalculatorService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<CalculationResultPayload> Handle(CombinedWithCalculationRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Calculating probability for A: {ProbabilityA} and B: {ProbabilityB} using the CombinedWith method",
            request.ProbabilityA, request.ProbabilityB);

        var result =
            await _service.CalculateProbabilityAsync(new ProbabilityParams(request.ProbabilityA,
                request.ProbabilityB));

        _logger.LogInformation("Calculation result: {Result} for the CombinedWith method", result);

        return await Task.FromResult(new CalculationResultPayload(result));
    }
}