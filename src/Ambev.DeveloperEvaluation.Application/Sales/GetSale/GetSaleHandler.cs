using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Handles a request to retrieve a single sale by its identifier.
/// Uses caching to optimize reads and falls back to the repository when cache is missing.
/// </summary>
public class GetSaleHandler : IRequestHandler<GetSaleCommand, GetSaleCommandResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;

    /// <summary>
    /// Initializes a new instance of the GetSaleHandler class.
    /// </summary>
    /// <param name="saleRepository">Repository used to access sale data.</param>
    /// <param name="mapper">Mapper used to convert domain entities to result DTOs.</param>
    /// <param name="cacheService">Cache service used to store and retrieve sales by id.</param>
    public GetSaleHandler(ISaleRepository saleRepository, IMapper mapper, ICacheService cacheService)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _cacheService = cacheService;
    }

    /// <summary>
    /// Handles the retrieval of a sale, attempting cache first and then the repository.
    /// </summary>
    /// <param name="command">The request containing the unique sale identifier.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the operation to complete.</param>
    /// <returns>The sale data mapped to GetSaleCommandResult, or an unsuccessful result with errors.</returns>
    public async Task<GetSaleCommandResult> Handle(GetSaleCommand command, CancellationToken cancellationToken)
    {
        GetSaleCommandResult result = new();

        try
        {
            var cacheKey = $"Sale:{command.Id}";
            var cachedResult = await _cacheService.GetAsync<GetSaleCommandResult>(cacheKey, cancellationToken);
            if (cachedResult != null)
            {
                return cachedResult;
            }

            var existentSale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);

            if (existentSale != null)
            {
                result = _mapper.Map<GetSaleCommandResult>(existentSale);
                result.Success = true;
                await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10), cancellationToken);
            }
        }
        catch (Exception ex)
        {
            result.Errors.Add(ex.Message);
        }
        return result;
    }
}
