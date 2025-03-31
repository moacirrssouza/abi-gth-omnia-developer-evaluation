using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Represents a command to cancel a sale identified by a unique SaleId. It implements the IRequest interface for
/// handling responses.
/// </summary>
public class CancelSaleCommand : IRequest<CancelSaleResponse>
{
	/// <summary>
	/// Represents the unique identifier for a sale. It is of type Guid, ensuring a globally unique value.
	/// </summary>
	public Guid SaleId { get; set; }
}
