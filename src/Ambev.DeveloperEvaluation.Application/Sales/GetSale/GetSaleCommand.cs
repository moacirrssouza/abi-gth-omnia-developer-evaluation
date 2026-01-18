using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Represents a request to retrieve details for a specific sale by its unique identifier.
/// </summary>
/// <remarks>Use this command with a mediator to obtain information about a sale. The result contains the sale
/// details if found. This command is typically used in CQRS patterns to query sale data.</remarks>
public class GetSaleCommand : IRequest<GetSaleCommandResult>
{
    public Guid Id { get; }

    public GetSaleCommand(Guid id)
    {
        Id = id;
    }
}