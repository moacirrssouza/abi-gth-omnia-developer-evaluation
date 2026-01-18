using Ambev.DeveloperEvaluation.Application.Events;
using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

/// <summary>
/// Handles notifications when a sale is modified by processing the associated event and persisting it to the event
/// store.
/// </summary>
/// <remarks>This event handler is typically used within a MediatR pipeline to respond to changes in sale
/// entities. It logs the handling of each event and ensures that modifications are recorded for auditing or further
/// processing. The handler is registered to process instances of the SaleModifiedEvent notification.</remarks>
public class SaleModifiedEventHandler : INotificationHandler<SaleModifiedEvent>
{
    private readonly IEventStore _eventStore;
    private readonly ILogger<SaleModifiedEventHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the SaleModifiedEventHandler class with the specified event store and logger.
    /// </summary>
    /// <param name="eventStore">The event store used to persist and retrieve event data. Cannot be null.</param>
    /// <param name="logger">The logger used to record operational and error information. Cannot be null.</param>
    public SaleModifiedEventHandler(IEventStore eventStore, ILogger<SaleModifiedEventHandler> logger)
    {
        _eventStore = eventStore;
        _logger = logger;
    }

    /// <summary>
    /// Handles a SaleModifiedEvent by persisting the event to the event store asynchronously.
    /// </summary>
    /// <param name="notification">The SaleModifiedEvent instance containing information about the modified sale. Cannot be null.</param>
    /// <param name="cancellationToken">A token that can be used to request cancellation of the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous handling operation.</returns>
    public async Task Handle(SaleModifiedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling SaleModifiedEvent for Sale {SaleId}", notification.Sale.Id);
        await _eventStore.SaveEventAsync(notification, cancellationToken);
    }
}