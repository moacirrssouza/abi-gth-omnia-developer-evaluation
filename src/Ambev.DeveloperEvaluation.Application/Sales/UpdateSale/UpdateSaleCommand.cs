using System.Text.Json.Serialization;
using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Represents a sale.
/// </summary>
public class UpdateSaleCommand : IRequest<UpdateSaleResult>
{
	/// <summary>
	/// The sale's unique identifier.
	/// </summary>
	[JsonIgnore]
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
	/// Updates the details of a sale transaction including its status and items.
	/// </summary>
	/// <param name="id">Identifies the specific sale transaction to be updated.</param>
	/// <param name="saleDate">Specifies the date when the sale occurred.</param>
	/// <param name="customerId">Represents the unique identifier for the customer involved in the sale.</param>
	/// <param name="branchId">Indicates the branch where the sale took place.</param>
	/// <param name="isCancelled">Indicates whether the sale transaction has been cancelled.</param>
	/// <param name="saleItems">Contains the list of items included in the sale transaction.</param>
	public UpdateSaleCommand(Guid id, DateTime saleDate, Guid customerId, Guid branchId, bool isCancelled, List<SaleItem> saleItems)
	{
		Id = id;
		SaleDate = saleDate;
		CustomerId = customerId;
		BranchId = branchId;
		IsCancelled = isCancelled;
		SaleItems = saleItems;
	}
}