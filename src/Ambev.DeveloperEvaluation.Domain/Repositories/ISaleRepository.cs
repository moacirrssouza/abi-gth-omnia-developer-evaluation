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
	/// Retrieves a sale based on the provided sale object.
	/// </summary>
	/// <param name="sale">The sale object containing the identifier and/or criteria to retrieve the sale.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the sale if found; otherwise, null.</returns>
	Task<IEnumerable<Sale>> GetListAsync(CancellationToken cancellationToken = default);

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
}
