using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// AutoMapper profile for UpdateSale feature.
/// </summary>
public class UpdateSaleProfile : Profile
{
    /// <summary>
    /// Initializes the rules for UpdateSale feature AutoMapper profile.
    /// </summary>
    public UpdateSaleProfile()
    {
        CreateMap<UpdateSaleRequest, UpdateSaleCommand>();
        CreateMap<UpdateSaleCommandResult, UpdateSaleResponse>();
        CreateMap<UpdateSaleItemRequest, UpdateSaleItemCommand>();
    }
}