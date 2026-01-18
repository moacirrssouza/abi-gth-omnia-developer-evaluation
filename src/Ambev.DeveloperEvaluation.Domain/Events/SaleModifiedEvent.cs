using Ambev.DeveloperEvaluation.Domain.Entities;

﻿namespace Ambev.DeveloperEvaluation.Domain.Events;
/// <summary>
/// Domain event raised when a sale is modified.
/// </summary>
public class SaleModifiedEvent : IDomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SaleModifiedEvent"/> class.
    /// </summary>
    /// <param name="sale">The modified sale.</param>
    public SaleModifiedEvent(Sale sale)
    {
        Sale = sale;
    }

    public Sale Sale { get; }
}