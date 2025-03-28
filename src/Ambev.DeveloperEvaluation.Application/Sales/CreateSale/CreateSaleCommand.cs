using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Represents a sale.
/// </summary>
public class CreateSaleCommand : IRequest<CreateSaleResult>
{
    /// <summary>
    /// The customer's unique identifier.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// The branch's unique identifier.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// The total amount of the sale.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// The total amount of the sale.
    /// </summary>
    public bool IsCancelled { get; set; } = false;
}