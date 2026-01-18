using Ambev.DeveloperEvaluation.Application.Events;
using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

/// <summary>
/// Handles the processing of <see cref="SaleCreatedEvent"/> notifications by persisting event data and logging the
/// operation.
/// </summary>
/// <remarks>This event handler is typically used within a MediatR pipeline to respond to sale creation events. It
/// relies on an event store for persistence and uses logging to record event handling activity. The handler is designed
/// to be registered with dependency injection and invoked automatically when a <see cref="SaleCreatedEvent"/> is
/// published.</remarks>
public class SaleCreatedEventHandler : INotificationHandler<SaleCreatedEvent>
{
    private readonly IEventStore _eventStore;
    private readonly ILogger<SaleCreatedEventHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the SaleCreatedEventHandler class with the specified event store and logger.
    /// </summary>
    /// <param name="eventStore">The event store used to persist and retrieve event data. Cannot be null.</param>
    /// <param name="logger">The logger used to record diagnostic and operational information. Cannot be null.</param>
    public SaleCreatedEventHandler(IEventStore eventStore, ILogger<SaleCreatedEventHandler> logger)
    {
        _eventStore = eventStore;
        _logger = logger;
    }

    public async Task Handle(SaleCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling SaleCreatedEvent for Sale {SaleId}", notification.Sale.Id);
        await _eventStore.SaveEventAsync(notification, cancellationToken);
    }
}