using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
{
	/// <summary>
	/// Validator for the UpdateSaleCommand.
	/// </summary>
	/// <remarks>
	/// Validation rules include:
	/// - CustomerId: Must not be empty.
	/// - BranchId: Must not be empty.
	/// - SaleItems: Must contain at least one sale item.
	/// - TotalAmount: Must be greater than or equal to 0.
	/// </remarks>
	public UpdateSaleRequestValidator()
	{
		RuleFor(sale => sale.CustomerId)
			   .NotEmpty().WithMessage("CustomerId must not be empty.");
		RuleFor(sale => sale.BranchId)
			.NotEmpty().WithMessage("BranchId must not be empty.");
		RuleFor(sale => sale.TotalAmount)
			.GreaterThanOrEqualTo(0)
			.WithMessage("TotalAmount must be greater than or equal to 0.");
	}
}