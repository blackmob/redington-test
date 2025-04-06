using Microsoft.Extensions.Logging;
using Moq;
using Redington.ProbabilityCalculator.Core.Handlers;
using Redington.ProbabilityCalculator.Core.Models;
using Redington.ProbabilityCalculator.Core.Requests;
using Redington.ProbabilityCalculator.Core.Services;

namespace Redington.ProbabilityCalculator.Tests.Handlers;

public class CombinedWithCalculationHandlerTest
{
    private readonly Mock<ICombinedWithCalculatorService> _calculatorMock;
    private readonly Mock<ILogger<CombinedWithCalculationHandler>> _loggerMock;

    public CombinedWithCalculationHandlerTest()
    {
        _loggerMock = new Mock<ILogger<CombinedWithCalculationHandler>>();
        _calculatorMock = new Mock<ICombinedWithCalculatorService>();
    }

    [Fact]
    public async Task CombinedWithCalculationHandler_ShouldReturnCorrectResult()
    {
        // Arrange
        _calculatorMock.Setup(t => t.CalculateProbabilityAsync(It.IsAny<IProbabilityParams>())).ReturnsAsync(0.75);

        var handler = new CombinedWithCalculationHandler(_loggerMock.Object, _calculatorMock.Object);
        var input = new CombinedWithCalculationRequest(0.5, 0.5);

        // Act
        var result = await handler.Handle(input, CancellationToken.None);

        // Assert
        _calculatorMock.Verify(t => t.CalculateProbabilityAsync(It.IsAny<IProbabilityParams>()), Times.Once);
        _loggerMock.Verify(
            t => t.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Exactly(2));
        Assert.Equal(0.75, result.CalculatedProbability);
    }
}