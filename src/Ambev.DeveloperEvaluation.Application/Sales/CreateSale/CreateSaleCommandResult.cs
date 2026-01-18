using Ambev.DeveloperEvaluation.Application.Base;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Represents the result of a create sale operation, including the unique identifier of the newly created sale.
/// </summary>
public class CreateSaleCommandResult : BaseResult
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public Guid Id { get; set; }
}