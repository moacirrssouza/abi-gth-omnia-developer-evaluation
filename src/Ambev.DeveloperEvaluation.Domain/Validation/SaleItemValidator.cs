using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

/// <summary>
/// Validator for the SaleItem entity.
/// </summary>
/// <remarks>
/// This validator ensures that:
/// - ProductId is not empty.
/// - Quantity is greater than zero and does not exceed 20.
/// - UnitPrice is greater than zero.
/// - TotalItemAmount is non-negative.
/// </remarks>
public class SaleItemValidator : AbstractValidator<SaleItem>
{
	/// <summary>
	/// Initializes a new instance of the SaleItemValidator class and defines validation rules for SaleItem.
	/// </summary>
	public SaleItemValidator()
	{
		RuleFor(item => item.ProductId)
			.NotEmpty()
			.WithMessage("ProductId must not be empty.");
		RuleFor(item => item.Quantity)
			.GreaterThan(0)
			.WithMessage("Quantity must be greater than zero.")
			.LessThanOrEqualTo(20).WithMessage("Cannot sell more than 20 identical items.");
		RuleFor(item => item.UnitPrice)
			.GreaterThan(0)
			.WithMessage("UnitPrice must be greater than zero.");
		RuleFor(item => item.TotalItemAmount)
			.GreaterThanOrEqualTo(0)
			.WithMessage("Total item amount must be non-negative.");
	}
}