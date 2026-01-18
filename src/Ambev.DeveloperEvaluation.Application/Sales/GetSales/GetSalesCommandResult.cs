using Ambev.DeveloperEvaluation.Application.Base;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Represents the result of a get sales command.
/// </summary>
public class GetSalesCommandResult : BaseResult
{
    public IEnumerable<GetSaleCommandResult>? Sales { get; set; }
}