using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// Profile for mapping between GetSaleCommand and GetSaleResponse
/// </summary>
public class GetSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetSaleCommand
    /// </summary>
    public GetSaleProfile()
    {
        CreateMap<Guid, GetSaleCommand>().ConstructUsing(id => new GetSaleCommand(id));
        CreateMap<GetSaleCommandResult, GetSaleResponse>();
        CreateMap<GetSaleItemCommandResult, GetSaleItemResponse>();
    }
}