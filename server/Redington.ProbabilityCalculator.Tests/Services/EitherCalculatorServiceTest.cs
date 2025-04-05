using FluentValidation;
using Redington.ProbabilityCalculator.Core.Models;
using Redington.ProbabilityCalculator.Core.Services;
using Redington.ProbabilityCalculator.Core.Validators;

namespace Redington.ProbabilityCalculator.Tests.Services;

public class EitherCalculatorServiceTest
{
    [Theory]
    [InlineData(0.5d, 0.5d, 0.75f)]
    [InlineData(0.0d, 0.0d, 0.0f)]
    [InlineData(0.0d, 1.0d, 1.0f)]
    [InlineData(1.0d, 0.0d, 1.0f)]
    [InlineData(1.0d, 1.0d, 1.0f)]
    [InlineData(0.5d, 0.0d, 0.5f)]
    public async Task CalculateEitherProbability_ShouldReturnExpectedResult(double probabilityA, double probabilityB,
        double expected)
    {
        // Arrange
        var calculatorService = new EitherCalculatorService(new ProbabilityParamsValidator());

        // Act
        var result =
            await calculatorService.CalculateProbabilityAsync(new ProbabilityParams(probabilityA, probabilityB));

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task CalculateEitherProbability_ShouldThrowValidationException_WhenProbabilityAIsLessThanZero()
    {
        // Arrange
        var calculatorService = new EitherCalculatorService(new ProbabilityParamsValidator());

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            calculatorService.CalculateProbabilityAsync(new ProbabilityParams(-0.1d, 0.5d)));
    }

    [Fact]
    public async Task CalculateEitherProbability_ShouldThrowValidationException_WhenProbabilityAIsGreaterThanOne()
    {
        // Arrange
        var calculatorService = new EitherCalculatorService(new ProbabilityParamsValidator());

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            calculatorService.CalculateProbabilityAsync(new ProbabilityParams(1.1d, 0.5d)));
    }

    [Fact]
    public async Task CalculateEitherProbability_ShouldThrowValidationException_WhenProbabilityBIsLessThanZero()
    {
        // Arrange
        var calculatorService = new EitherCalculatorService(new ProbabilityParamsValidator());

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            calculatorService.CalculateProbabilityAsync(new ProbabilityParams(0.5d, -0.1d)));
    }

    [Fact]
    public async Task CalculateEitherProbability_ShouldThrowValidationException_WhenProbabilityBIsGreaterThanOne()
    {
        // Arrange
        var calculatorService = new EitherCalculatorService(new ProbabilityParamsValidator());

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            calculatorService.CalculateProbabilityAsync(new ProbabilityParams(0.5d, 1.1d)));
    }
}