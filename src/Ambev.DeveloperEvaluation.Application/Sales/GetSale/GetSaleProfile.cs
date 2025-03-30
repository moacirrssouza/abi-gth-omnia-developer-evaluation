using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Profile for mapping between Sale entity and GetSaleResponse
/// </summary>
public class GetSaleProfile : Profile
{
	/// <summary>
	/// Initializes the mappings for GetSale operation
	/// </summary>
	public GetSaleProfile()
	{
		CreateMap<Sale, GetSaleResult>();
		CreateMap<SaleItem, SaleItem>()
			.ForMember(dest => dest.TotalItemAmount,
				opt => opt.MapFrom(src => src.Quantity * src.UnitPrice - src.Discount));
	}
}