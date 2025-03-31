using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IUserRepository using Entity Framework Core
/// </summary>
public class SaleRepository : ISaleRepository
{
	private readonly DefaultContext _context;

	/// <summary>
	/// Initializes a new instance of SaleRepository
	/// </summary>
	/// <param name="context">The database context</param>
	public SaleRepository(DefaultContext context)
	{
		_context = context;
	}

	/// <summary>
	/// Creates a new sale in the database
	/// </summary>
	/// <param name="sale">The sale to create</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>The created user</returns>
	public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
	{
		await _context.Sales.AddAsync(sale, cancellationToken);
		await _context.SaveChangesAsync(cancellationToken);
		return sale;
	}
	
	/// <summary>
	/// Retrieves a queryable collection of sales, including their associated sale items.
	/// </summary>
	/// <returns>Returns an IQueryable of Sale objects with included SaleItems.</returns>
	public IQueryable<Sale> GetListQueryableSales(CancellationToken cancellationToken = default)
	{
		return _context.Sales.Include(si => si.SaleItems).OrderByDescending(s => s.SaleDate).AsQueryable();
	}

	/// <summary>
	/// Retrieves a sale by their unique identifier
	/// </summary>
	/// <param name="id">The unique identifier of the sale</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>The sale if found, null otherwise</returns>
	public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
	{
		return await _context.Sales.Include(si => si.SaleItems)
			.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
	}

	/// <summary>
	/// Deletes a sale from the database
	/// </summary>
	/// <param name="id">The unique identifier of the sale to delete</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>True if the sale was deleted, false if not found</returns>
	public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var sale = await _context.Sales
			.Include(s => s.SaleItems)
			.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

		if (sale == null)
			return false;

		_context.Sales.Remove(sale);
		await _context.SaveChangesAsync(cancellationToken);
		return true;
	}

	/// <summary>
	/// Updates an existing sale in the database
	/// </summary>
	/// <param name="sale">The sale to update</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>The updated sale</returns>
	public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
	{
		var existingSale = await _context.Sales
			.Include(s => s.SaleItems)
			.FirstOrDefaultAsync(s => s.Id == sale.Id, cancellationToken);

		if (existingSale == null)
			throw new Exception("Sale not found");

		_context.Update(existingSale);
		await _context.SaveChangesAsync(cancellationToken);
		return existingSale;
	}
	
	/// <summary>
	/// Cancels a sale identified by a unique identifier and updates the database accordingly.
	/// </summary>
	/// <param name="saleId">The unique identifier for the sale that needs to be cancelled.</param>
	/// <param name="cancellationToken">Used to signal the cancellation of the operation if needed.</param>
	/// <returns>Returns true if the sale was successfully cancelled, otherwise false.</returns>
	public async Task<bool> CancelSaleAsync(Guid saleId, CancellationToken cancellationToken)
	{
		var sale = await _context.Sales
			.Include(s => s.SaleItems)
			.FirstOrDefaultAsync(s => s.Id == saleId, cancellationToken);

		if (sale == null)
			return false;

		sale.IsCancelled = true;
		foreach (var item in sale.SaleItems)
		{
			item.IsCancelled = true;
		}

		_context.Sales.Update(sale);
		await _context.SaveChangesAsync(cancellationToken);
		return true;
	}
}