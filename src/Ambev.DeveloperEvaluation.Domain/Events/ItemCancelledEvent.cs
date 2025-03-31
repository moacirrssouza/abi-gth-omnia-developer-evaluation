namespace Ambev.DeveloperEvaluation.Domain.Events;

public class ItemCancelledEvent : SaleEvent
{
	public Guid ItemId { get; }
	public ItemCancelledEvent(Guid itemId)
	{
		ItemId = itemId;
	}
}
