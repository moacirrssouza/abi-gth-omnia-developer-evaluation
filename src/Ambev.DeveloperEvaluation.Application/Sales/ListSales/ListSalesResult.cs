using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

/// <summary>
/// Response model for ListSales operation
/// </summary>
public class ListSalesResult
{
	/// <summary>
	/// The sale's unique identifier.
	/// </summary>
	public Guid Id { get; set; }

	/// <summary>
	/// The date and time the sale was made.
	/// </summary>
	public DateTime SaleDate { get; set; } = DateTime.UtcNow;

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
	/// Indicates whether an operation has been cancelled. Defaults to false.
	/// </summary>
	public bool IsCancelled { get; set; } = false;
}