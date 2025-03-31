using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Application.Common.Events;

public interface IEventPublisher
{
	Task PublishAsync<T>(T saleEvent) where T : SaleEvent;
}
