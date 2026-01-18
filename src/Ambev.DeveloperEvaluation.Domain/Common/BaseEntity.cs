using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Events;

﻿namespace Ambev.DeveloperEvaluation.Domain.Common;
/// <summary>
/// Base class for domain entities.
/// </summary>
public class BaseEntity : IComparable<BaseEntity>
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public Guid Id { get; set; }
    private readonly List<IDomainEvent> _domainEvents = new();
    /// <summary>
    /// Gets the read-only collection of domain events associated with the entity.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Validates the entity asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validation errors.</returns>
    public Task<IEnumerable<ValidationErrorDetail>> ValidateAsync()
    {
        return Validator.ValidateAsync(this);
    }
    /// <summary>
    /// Adds a domain event to the entity.
    /// </summary>
    /// <param name="domainEvent">The domain event to add.</param>
    public void AddEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    /// <summary>
    /// Clears all domain events associated with the entity.
    /// </summary>
    public void ClearEvents()
    {
        _domainEvents.Clear();
    }
    /// <summary>
    /// Compares the current entity with another entity based on their IDs.
    /// </summary>
    /// <param name="other">The entity to compare with.</param>
    /// <returns>A value that indicates the relative order of the entities.</returns>
    public int CompareTo(BaseEntity? other)
    {
        if (other == null)
        {
            return 1;
        }

        return other!.Id.CompareTo(Id);
    }
}