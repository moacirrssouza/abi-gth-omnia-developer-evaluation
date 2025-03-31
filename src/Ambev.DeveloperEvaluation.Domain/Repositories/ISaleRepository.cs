using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Sale entity operations
/// </summary>
public interface ISaleRepository
{
	/// <summary>
	/// Creates a new sale in the repository
	/// </summary>
	/// <param name="sale">The sale to create</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>The created sale</returns>
	Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);
	
	/// <summary>
	/// Retrieves a queryable collection of sales data.
	/// </summary>
	/// <returns>Returns an IQueryable interface for querying sales.</returns>
	IQueryable<Sale> GetListQueryableSales(CancellationToken cancellationToken = default);
	
	/// <summary>
	/// Retrieves a sale by their unique identifier
	/// </summary>
	/// <param name="id">The unique identifier of the usale</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>The sale if found, null otherwise</returns>
	Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes a sale from the repository
	/// </summary>
	/// <param name="id">The unique identifier of the sale to delete</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>True if the sale was deleted, false if not found</returns>
	Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing sale in the repository.
	/// </summary>
	/// <param name="sale">The sale entity with updated data.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>The updated sale if successful, or null if the sale was not found.</returns>
	Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default);
	
	/// <summary>
	/// Cancels a sale asynchronously based on a unique identifier. It returns a boolean indicating the success of the
	/// cancellation.
	/// </summary>
	/// <param name="saleId">The unique identifier for the sale that needs to be canceled.</param>
	/// <param name="cancellationToken">Used to signal the cancellation of the operation if needed.</param>
	/// <returns>A boolean value that indicates whether the cancellation was successful.</returns>
	Task<bool> CancelSaleAsync(Guid saleId, CancellationToken cancellationToken);
}