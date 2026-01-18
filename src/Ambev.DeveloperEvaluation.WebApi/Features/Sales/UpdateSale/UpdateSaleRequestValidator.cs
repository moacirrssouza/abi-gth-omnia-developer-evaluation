using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Validator for UpdateSaleRequest.
/// </summary>
public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
{
    /// <summary>
    /// Initializes the rules for UpdateSaleRequestValidator.
    /// </summary>
    public UpdateSaleRequestValidator()
    {
        RuleFor(x => x.Date).NotEqual(default(DateTime));
        RuleFor(x => x.Customer).NotEmpty();
        RuleFor(x => x.Branch).NotEmpty();
        RuleFor(x => x.Items).NotNull().Must(i => i.Count > 0);
        RuleForEach(x => x.Items).ChildRules(items =>
        {
            items.RuleFor(i => i.Product).NotEmpty();
            items.RuleFor(i => i.Quantity).GreaterThan(0);
            items.RuleFor(i => i.UnitPrice).GreaterThan(0);
        });
    }
}