namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleModifiedEvent : SaleEvent
{
	public Guid SaleId { get; }
	public SaleModifiedEvent(Guid saleId)
	{
		SaleId = saleId;
	}
}
