namespace Ambev.DeveloperEvaluation.Domain.Events;

public abstract class SaleEvent
{
	public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
}
