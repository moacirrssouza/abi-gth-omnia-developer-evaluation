using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Domain event raised when an item is cancelled.
/// </summary>
public class ItemCancelledEvent : IDomainEvent
{
    public ItemCancelledEvent(SaleItem item, Sale sale)
    {
        Item = item;
        Sale = sale;
    }

    public SaleItem Item { get; }
    public Sale Sale { get; }
}