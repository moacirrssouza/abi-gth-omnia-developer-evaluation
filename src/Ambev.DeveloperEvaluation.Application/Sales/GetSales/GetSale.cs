using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Represents a request to retrieve a paginated list of sales records.
/// </summary>
/// <remarks>Use this command to specify pagination parameters when querying for sales data. The <see
/// cref="Skip"/> and <see cref="Take"/> properties control which subset of sales records is returned. This command is
/// typically used with a mediator pattern to decouple the request from its handling logic.</remarks>
public class GetSalesCommand : IRequest<GetSalesCommandResult>
{
    public int? Skip { get; }

    public int? Take { get; }

    public GetSalesCommand(int? skip, int? take)
    {
        Skip = skip;
        Take = take;
    }
}