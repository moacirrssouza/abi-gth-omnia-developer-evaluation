using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Handles requests to delete a sale by processing a <see cref="DeleteSaleCommand"/> and returning the result.
/// </summary>
/// <remarks>This handler coordinates the deletion of a sale entity using the provided sale repository. It logs
/// critical errors encountered during the operation. Typically used within a MediatR pipeline to encapsulate the delete
/// operation for sales.</remarks>
public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, DeleteSaleCommandResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<DeleteSaleHandler> _logger;
    private readonly string objectName = nameof(DeleteSaleHandler);

    /// <summary>
    /// Initializes a new instance of the DeleteSaleHandler class with the specified sale repository and logger.
    /// </summary>
    /// <param name="saleRepository">The repository used to access and manage sale records. Cannot be null.</param>
    /// <param name="logger">The logger used to record diagnostic and operational information. Cannot be null.</param>
    public DeleteSaleHandler(ISaleRepository saleRepository, ILogger<DeleteSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _logger = logger;
    }

    /// <summary>
    /// Handles the deletion of a sale based on the specified command.
    /// </summary>
    /// <param name="command">The command containing the identifier of the sale to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a DeleteSaleCommandResult indicating
    /// whether the deletion was successful and any errors that occurred.</returns>
    public async Task<DeleteSaleCommandResult> Handle(DeleteSaleCommand command, CancellationToken cancellationToken)
    {

        DeleteSaleCommandResult result = new();

        try
        {
            await _saleRepository.DeleteAsync(command.Id, cancellationToken);
            result.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"[{objectName}] - Error during delete handler: {ex.Message}", ex);
            result.Errors.Add(ex.Message);
        }

        return result;
    }
}