using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Application.Events;

/// <summary>
/// Defines a contract for persisting domain events asynchronously.
/// </summary>
/// <remarks>Implementations of this interface are responsible for storing events that represent changes in domain
/// state. Event stores are typically used in event sourcing architectures to record and retrieve the sequence of events
/// that have occurred within a system.</remarks>
public interface IEventStore
{
    /// <summary>
    /// Saves a domain event asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the domain event. Must implement <see cref="IDomainEvent"/>.</typeparam>
    /// <param name="event">The domain event to be saved.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the saved domain event.</returns>
    Task SaveEventAsync<T>(T @event, CancellationToken cancellationToken = default) where T : IDomainEvent;
}