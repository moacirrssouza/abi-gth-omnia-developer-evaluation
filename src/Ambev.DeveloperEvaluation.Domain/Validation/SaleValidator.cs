using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for the Sale entity.
/// </summary>
public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        RuleFor(sale => sale.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID is required.");
        RuleFor(sale => sale.BranchId)
            .NotEmpty()
            .WithMessage("Branch ID is required.");
        RuleFor(sale => sale.SaleDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Sale date cannot be in the future.");
        RuleFor(sale => sale.TotalAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Total amount must be non-negative.");
    }
}