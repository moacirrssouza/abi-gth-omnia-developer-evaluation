using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Represents a request to create a new sale in the system.
/// </summary>
public class CreateSaleRequest
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
	public decimal TotalAmount => SaleItems.Sum(i => i.TotalItemAmount);

	/// <summary>
	/// Indicates whether a process or operation has been cancelled. Defaults to false, meaning not cancelled.
	/// </summary>
	public bool IsCancelled { get; set; } = false;
	
	/// <summary>
	/// The items of the sale.
	/// </summary>
	public List<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}