using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class UserCreatedEvent : IDomainEvent
{
    public UserCreatedEvent(User user)
    {
        User = user;
    }

    public User User { get; }
}