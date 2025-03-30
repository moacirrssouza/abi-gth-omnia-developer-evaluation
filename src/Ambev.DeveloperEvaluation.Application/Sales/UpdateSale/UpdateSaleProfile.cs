using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Profile for mapping between Sale entity and UpdateSaleResponse
/// </summary>
public class UpdateSaleProfile : Profile
{
	/// <summary>
	/// Initializes the mappings for UpdateSale operation
	/// </summary>
	public UpdateSaleProfile()
	{
		CreateMap<UpdateSaleCommand, Sale>();
		CreateMap<Sale, UpdateSaleResult>();
		CreateMap<Sale, UpdateSaleCommand>()
		   .ConstructUsing(src => new UpdateSaleCommand(
			   src.Id,
			   src.SaleDate,
			   src.CustomerId,
			   src.BranchId,
			   src.IsCancelled,
			   src.SaleItems
		   ));
		CreateMap<SaleItem, SaleItem>()
			.ForMember(dest => dest.TotalItemAmount,
				opt => opt.MapFrom(src => src.Quantity * src.UnitPrice - src.Discount));
	}
}