namespace Redington.ProbabilityCalculator.Core.Services;

/// <summary>
///     The main interface that any service class must implement.
///     Also requires any service to implement <see cref="IAsyncDisposable" />
///     so that it can be disposed successfully by the .NET DI.
/// </summary>
public interface IService : IAsyncDisposable
{
}