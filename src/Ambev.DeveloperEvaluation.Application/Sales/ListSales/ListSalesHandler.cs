using Ambev.DeveloperEvaluation.Application.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

/// <summary>
/// Handler responsible for processing the command to list sale.
/// </summary>
public class ListSalesHandler : IRequestHandler<ListSalesCommand, PaginatedList<ListSalesResult>>
{
	private readonly ISaleRepository _saleRepository;
	private readonly IMapper _mapper;

	/// <summary>
	/// Initializes a new instance of the <see cref="ListSalesHandler"/> class with the specified dependencies.
	/// </summary>
	/// <param name="saleRepository">Repository for accessing sale data.</param>
	/// <param name="mapper">Object mapper for converting domain entities to DTOs.</param>
	public ListSalesHandler(ISaleRepository saleRepository, IMapper mapper)
	{
		_saleRepository = saleRepository;
		_mapper = mapper;
	}

	/// <summary>
	/// Handles the command to list sale, returning a collection of mapped sale results.
	/// </summary>
	/// <param name="command">The command object that triggers the sale listing process.</param>
	/// <param name="cancellationToken">Token used to cancel the asynchronous operation if needed.</param>
	/// <returns>A collection of <see cref="ListSalesResult"/> representing the retrieved sales.</returns>
	public async Task<PaginatedList<ListSalesResult>> Handle(ListSalesCommand command, CancellationToken cancellationToken)
	{
		var queryable = _saleRepository.GetListQueryableSales();
		var paginatedSales = await PaginatedList<Sale>.CreateAsync(queryable, command.PageNumber, command.PageSize);
		var resultList = _mapper.Map<List<ListSalesResult>>(paginatedSales);
		var paginatedResult = new PaginatedList<ListSalesResult>(
			resultList,
			paginatedSales.TotalCount,
			paginatedSales.CurrentPage,
			paginatedSales.PageSize
		);

		return paginatedResult;
	}
}