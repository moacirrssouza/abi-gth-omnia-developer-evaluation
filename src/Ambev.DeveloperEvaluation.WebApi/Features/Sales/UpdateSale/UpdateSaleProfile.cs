using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

public class UpdateSaleProfile : Profile
{
	public UpdateSaleProfile()
	{
		CreateMap<UpdateSaleRequest, UpdateSaleCommand>();
		CreateMap<UpdateSaleRequest, UpdateSaleCommand>()
		   .ConstructUsing(src => new UpdateSaleCommand(
			   src.Id,
			   src.SaleDate,
			   src.CustomerId,
			   src.BranchId,
			   src.IsCancelled,
			   src.SaleItems
		   ));
	}
}