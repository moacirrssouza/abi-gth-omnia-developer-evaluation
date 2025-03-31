using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Common.Events;

public class EventPublisher : IEventPublisher
{
	private readonly ILogger<EventPublisher> _logger;

	public EventPublisher(ILogger<EventPublisher> logger)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public async Task PublishAsync<T>(T saleEvent) where T : SaleEvent
	{
		if (saleEvent == null)
		{
			_logger.LogWarning("⚠️ Attempt to publish a null event.");
			return;
		}

		try
		{
			_logger.LogInformation(
				"📢 Publishing event: {EventType} | Occurred on: {OccurredOn} | Details: {@saleEvent}",
				saleEvent.GetType().Name,
				saleEvent.OccurredOn,
				saleEvent
			);

			await Task.Delay(100);
			_logger.LogInformation("✅ Event {EventType} published successfully!", saleEvent.GetType().Name);
		}
		catch (Exception ex)
		{
			_logger.LogError(
				ex,
				"❌ Error publishing event {EventType}: {Message}",
				saleEvent.GetType().Name,
				ex.Message
			);
			throw;
		}
	}
}