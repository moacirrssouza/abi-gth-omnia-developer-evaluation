using Ambev.DeveloperEvaluation.Application.Events;
using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

/// <summary>
/// Handles the processing of sale cancellation events by persisting the event and logging the operation.
/// </summary>
/// <remarks>This event handler is typically used within a MediatR pipeline to respond to sale cancellation
/// events. It ensures that each cancellation is recorded in the event store and that relevant information is logged for
/// auditing or troubleshooting purposes.</remarks>
public class SaleCancelledEventHandler : INotificationHandler<SaleCancelledEvent>
{
    private readonly IEventStore _eventStore;
    private readonly ILogger<SaleCancelledEventHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the SaleCancelledEventHandler class with the specified event store and logger.
    /// </summary>
    /// <param name="eventStore">The event store used to persist and retrieve event data related to sale cancellations. Cannot be null.</param>
    /// <param name="logger">The logger used to record operational and diagnostic information for the event handler. Cannot be null.</param>
    public SaleCancelledEventHandler(IEventStore eventStore, ILogger<SaleCancelledEventHandler> logger)
    {
        _eventStore = eventStore;
        _logger = logger;
    }

    /// <summary>
    /// Handles a sale cancellation event by persisting the event to the event store.
    /// </summary>
    /// <param name="notification">The sale cancellation event to handle. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task Handle(SaleCancelledEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling SaleCancelledEvent for Sale {SaleId}", notification.Sale.Id);
        await _eventStore.SaveEventAsync(notification, cancellationToken);
    }
}