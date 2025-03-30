using Ambev.DeveloperEvaluation.Application.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

/// <summary>
/// Represents a command to list sales, including details like sale ID, date, customer ID, branch ID, and total
/// amount. Also tracks sale items and cancellation status.
/// </summary>
public class ListSalesCommand : IRequest<PaginatedList<ListSalesResult>>
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
	public decimal TotalAmount => SaleItems.Sum(i => i.TotalItemAmount);

	/// <summary>
	/// Indicates whether an operation has been cancelled. Defaults to false.
	/// </summary>
	public bool IsCancelled { get; set; } = false;
	
	/// <summary>
	/// The items of the sale.
	/// </summary>
	public List<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
	
	/// <summary>
	/// Represents the current page number in a paginated list. It is initialized to 1 by default.
	/// </summary>
	public int PageNumber { get; set; } = 1;

	/// <summary>
	/// Specifies the number of items to display per page. Defaults to 10.
	/// </summary>
	public int PageSize { get; set; } = 10;
}