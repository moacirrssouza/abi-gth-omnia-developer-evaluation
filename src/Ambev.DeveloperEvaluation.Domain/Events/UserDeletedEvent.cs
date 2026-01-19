namespace Ambev.DeveloperEvaluation.Domain.Events;

public class UserDeletedEvent : IDomainEvent
{
    public UserDeletedEvent(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}