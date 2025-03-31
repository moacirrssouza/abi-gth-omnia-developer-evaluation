namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleCancelledEvent : SaleEvent
{
	public Guid SaleId { get; }
	public SaleCancelledEvent(Guid saleId)
	{
		SaleId = saleId;
	}
}
