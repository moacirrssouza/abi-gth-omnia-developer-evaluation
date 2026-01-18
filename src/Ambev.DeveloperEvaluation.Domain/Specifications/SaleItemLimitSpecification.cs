using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Specifications;

/// <summary>
/// Represents a specification that determines whether a sale contains any product with a total quantity exceeding the
/// allowed item limit.
/// </summary>
/// <remarks>Use this specification to enforce business rules that restrict the maximum quantity of individual
/// products in a sale. This can help prevent overselling or ensure compliance with inventory policies.</remarks>
public class SaleItemLimitSpecification : ISpecification<Sale>
{
    /// <summary>
    /// Determines whether the specified sale contains more than 20 units of any single product.
    /// </summary>
    /// <param name="sale">The sale to evaluate. Cannot be null.</param>
    /// <returns>true if any product in the sale has a total quantity greater than 20; otherwise, false.</returns>
    public bool IsSatisfiedBy(Sale sale)
    {
        return (sale.Items.GroupBy(x => x.Product)
               .Any(group => group.Sum(x => x.Quantity) > 20));
    }
}