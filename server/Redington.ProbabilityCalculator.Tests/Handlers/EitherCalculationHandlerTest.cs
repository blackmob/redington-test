using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Redington.ProbabilityCalculator.Core.Commands;
using Redington.ProbabilityCalculator.Core.Handlers;
using Redington.ProbabilityCalculator.Core.Models;
using Redington.ProbabilityCalculator.Core.Services;

namespace Redington.ProbabilityCalculator.Tests.Handlers;

public class EitherCalculationHandlerTest
{
    private readonly Mock<ILogger<EitherCalculationHandler>> _loggerMock;
    private readonly Mock<IEitherCalculatorService> _calculatorMock;

    public EitherCalculationHandlerTest()
    {
        _loggerMock = new Mock<ILogger<EitherCalculationHandler>>();
        _calculatorMock = new Mock<IEitherCalculatorService>();
    }

    [Fact]
    public async Task EitherCalculationHandler_ShouldReturnCorrectResult()
    {
        // Arrange
        _calculatorMock.Setup(t => t.CalculateProbabilityAsync(It.IsAny<IProbabilityParams>())).ReturnsAsync(0.75);

        var handler = new EitherCalculationHandler(_loggerMock.Object, _calculatorMock.Object);
        var input = new EitherCalculationCommand(0.5, 0.5);

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