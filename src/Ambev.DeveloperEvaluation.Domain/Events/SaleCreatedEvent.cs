using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Domain event raised when a sale is created.
/// </summary>
public class SaleCreatedEvent : IDomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SaleCreatedEvent"/> class.
    /// </summary>
    /// <param name="sale">The created sale.</param>
    public SaleCreatedEvent(Sale sale)
    {
        Sale = sale;
    }

    public Sale Sale { get; }
}