using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Represents a request to delete a sale identified by its unique identifier.
/// </summary>
public class DeleteSaleCommand : IRequest<DeleteSaleCommandResult>
{
    /// <summary>
    /// Gets the unique identifier for this instance.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Initializes a new instance of the DeleteSaleCommand class with the specified sale identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the sale to be deleted.</param>
    public DeleteSaleCommand(Guid id)
    {
        Id = id;
    }
}