using Ambev.DeveloperEvaluation.Domain.Entities;

﻿namespace Ambev.DeveloperEvaluation.Domain.Events;
/// <summary>
/// Domain event raised when a sale is cancelled.
/// </summary>
public class SaleCancelledEvent : IDomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SaleCancelledEvent"/> class.
    /// </summary>
    /// <param name="sale">The cancelled sale.</param>
    public SaleCancelledEvent(Sale sale)
    {
        Sale = sale;
    }

    public Sale Sale { get; }
}