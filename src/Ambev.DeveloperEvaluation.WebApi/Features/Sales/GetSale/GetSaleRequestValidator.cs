using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// Validator for GetSaleRequest
/// </summary>
public class GetSaleRequestValidator : AbstractValidator<GetSaleRequest>
{
    /// <summary>
    /// Initializes the rules for GetSaleRequest validation.
    /// </summary>
    public GetSaleRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}