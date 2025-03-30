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
	/// - Each SaleItem is validated using the SaleItemValidator.
	/// </remarks>
	public UpdateSaleRequestValidator()
	{
		RuleFor(sale => sale.CustomerId)
			.NotEmpty().WithMessage("CustomerId must not be empty.");
		RuleFor(sale => sale.BranchId)
			.NotEmpty().WithMessage("BranchId must not be empty.");
		RuleFor(sale => sale.SaleItems)
			.NotEmpty().WithMessage("Sale must have at least one sale item.");
		RuleFor(sale => sale.TotalAmount)
			.GreaterThanOrEqualTo(0)
			.WithMessage("TotalAmount must be greater than or equal to 0.");
		RuleForEach(sale => sale.SaleItems)
			.SetValidator(new SaleItemValidator());
	}
}