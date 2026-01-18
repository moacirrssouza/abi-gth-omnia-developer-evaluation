using Ambev.DeveloperEvaluation.Application.Events;
using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

/// <summary>
/// Handles the processing of item cancellation events by persisting the event and logging the operation.
/// </summary>
/// <remarks>This event handler is typically used within a MediatR pipeline to respond to item cancellation events
/// in the system. It ensures that each cancellation is recorded in the event store and that relevant information is
/// logged for auditing or troubleshooting purposes.</remarks>
public class ItemCancelledEventHandler : INotificationHandler<ItemCancelledEvent>
{
    private readonly IEventStore _eventStore;
    private readonly ILogger<ItemCancelledEventHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the ItemCancelledEventHandler class with the specified event store and logger.
    /// </summary>
    /// <param name="eventStore">The event store used to persist and retrieve event data. Cannot be null.</param>
    /// <param name="logger">The logger used to record diagnostic and operational information. Cannot be null.</param>
    public ItemCancelledEventHandler(IEventStore eventStore, ILogger<ItemCancelledEventHandler> logger)
    {
        _eventStore = eventStore;
        _logger = logger;
    }

    /// <summary>
    /// Handles the cancellation of an item by processing the specified event and persisting it to the event store.
    /// </summary>
    /// <param name="notification">The event data representing the cancelled item. Cannot be null.</param>
    /// <param name="cancellationToken">A token that can be used to request cancellation of the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task Handle(ItemCancelledEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling ItemCancelledEvent for Sale {SaleId}, Item {ItemId}",
            notification.Sale.Id,
            notification.Item.Id);
        await _eventStore.SaveEventAsync(notification, cancellationToken);
    }
}