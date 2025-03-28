using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;

/// <summary>
/// Profile for mapping ListSalesProfile to Sale
/// </summary>
public class ListSalesProfile : Profile
{
	/// <summary>
	/// Initializes the mappings for ListSales feature
	/// </summary>
	public ListSalesProfile()
	{
		CreateMap<ListSalesRequest, ListSalesCommand>();
	}
}