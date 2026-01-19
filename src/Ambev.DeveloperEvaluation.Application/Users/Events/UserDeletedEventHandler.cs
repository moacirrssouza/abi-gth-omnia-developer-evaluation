using Ambev.DeveloperEvaluation.Application.Events;
using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Users.Events;

public class UserDeletedEventHandler : INotificationHandler<UserDeletedEvent>
{
    private readonly IEventStore _eventStore;
    private readonly ILogger<UserDeletedEventHandler> _logger;

    public UserDeletedEventHandler(IEventStore eventStore, ILogger<UserDeletedEventHandler> logger)
    {
        _eventStore = eventStore;
        _logger = logger;
    }

    public async Task Handle(UserDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UserDeletedEvent for User {UserId}", notification.UserId);
        await _eventStore.SaveEventAsync(notification, cancellationToken);
    }
}