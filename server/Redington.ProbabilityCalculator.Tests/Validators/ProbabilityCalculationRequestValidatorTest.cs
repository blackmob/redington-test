using Redington.ProbabilityCalculator.Core.Models;
using Redington.ProbabilityCalculator.Core.Requests;
using Redington.ProbabilityCalculator.Core.Validators;

namespace Redington.ProbabilityCalculator.Tests.Validators;

public class ProbabilityCalculationRequestValidatorTest
{
    [Theory]
    [InlineData(1.5, 0.5, "'Probability A' must be between 0 and 1. You entered 1.5.", false)]
    [InlineData(-0.5, 0.5, "'Probability A' must be between 0 and 1. You entered -0.5.", false)]
    [InlineData(0.5, 1.5, "'Probability B' must be between 0 and 1. You entered 1.5.", false)]
    [InlineData(0.5, -0.5, "'Probability B' must be between 0 and 1. You entered -0.5.", false)]
    [InlineData(0.5, 0.5, "", true)]
    public void ValidatorTests(double probabilityA, double probabilityB, string expectedErrorMessage, bool isValid)
    {
        // Arrange
        var validator = new ProbabilityCalculationRequestValidator();
        var request = new ProbabilityCalculationRequest(probabilityA, probabilityB, CalculationType.Either);

        // Act
        var result = validator.Validate(request);

        // Assert
        Assert.Equal(isValid, result.IsValid);

        if (isValid)
            Assert.Empty(result.Errors);
        else
            Assert.Equal(expectedErrorMessage, result.Errors[0].ErrorMessage);
    }
}