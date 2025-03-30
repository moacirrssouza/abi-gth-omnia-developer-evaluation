using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Entities;
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
    public decimal TotalAmount => SaleItems.Sum(i => i.TotalItemAmount);

    /// <summary>
    /// The total amount of the sale.
    /// </summary>
    public bool IsCancelled { get; set; } = false;
    
    /// <summary>
    /// The items of the sale.
    /// </summary>
    public List<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    
    public ValidationResultDetail Validate()
    {
        var validator = new CreateSaleCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}