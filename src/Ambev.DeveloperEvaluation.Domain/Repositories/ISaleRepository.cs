using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Defines a contract for managing sales data, including creation, retrieval, updating, and deletion of sale records.
/// </summary>
/// <remarks>Implementations of this interface are responsible for providing asynchronous access to persistent
/// storage of sales. Methods support cancellation via a CancellationToken. Thread safety and transaction management
/// depend on the specific implementation.</remarks>
public interface ISaleRepository
{
    /// <summary>
    /// Asynchronously creates a new sale record in the data store.
    /// </summary>
    /// <param name="sale">The sale to be created. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous create operation.</returns>
    Task CreateAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates the specified sale in the data store.
    /// </summary>
    /// <param name="sale">The sale entity to update. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the update operation.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a sale by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the sale to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the sale with the specified
    /// identifier, or <see langword="null"/> if no matching sale is found.</returns>
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a list of all sales, with optional pagination.
    /// </summary>
    /// <param name="skip">The number of sales to skip before starting to collect the results. Specify null to start from the beginning.</param>
    /// <param name="take">The maximum number of sales to return. Specify null to return all remaining sales after skipping.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of sales, or null if no sales
    /// are found.</returns>
    Task<List<Sale>?> GetAllAsync(int? skip, int? take, CancellationToken cancellationToken = default);


    /// <summary>
    /// Asynchronously deletes the entity with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task result is <see langword="true"/> if the
    /// entity was successfully deleted; otherwise, <see langword="false"/>.</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}