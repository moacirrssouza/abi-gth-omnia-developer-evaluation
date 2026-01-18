using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Handles requests to retrieve a paginated list of sales based on the specified command parameters.
/// </summary>
/// <remarks>This handler coordinates the retrieval of sales data from the repository and maps the results to the
/// appropriate command result type. It is typically used within a MediatR pipeline to process sales queries. The
/// handler returns a result object containing the sales data and any errors encountered during processing.</remarks>
public class GetSalesHandler : IRequestHandler<GetSalesCommand, GetSalesCommandResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the GetSalesHandler class with the specified sale repository and object mapper.
    /// </summary>
    /// <param name="saleRepository">The repository used to access and manage sale data. Cannot be null.</param>
    /// <param name="mapper">The object mapper used to convert between domain entities and data transfer objects. Cannot be null.</param>
    public GetSalesHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the specified sales retrieval command asynchronously and returns the result containing the requested
    /// sales data.
    /// </summary>
    /// <remarks>If an exception occurs during the retrieval process, the error message is added to the
    /// result's error collection, and the operation is marked as unsuccessful.</remarks>
    /// <param name="command">The command containing the parameters for retrieving sales, including pagination options such as the number of
    /// records to skip and take.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="GetSalesCommandResult"/> with the retrieved sales data and operation status.</returns>
    public async Task<GetSalesCommandResult> Handle(GetSalesCommand command, CancellationToken cancellationToken)
    {

        GetSalesCommandResult result = new();

        try
        {
            var pagedSales = await _saleRepository.GetAllAsync(command.Skip, command.Take, cancellationToken);

            if (pagedSales != null)
            {
                result.Sales = _mapper.Map<IEnumerable<GetSaleCommandResult>>(pagedSales);
                result.Success = true;
            }
        }
        catch (Exception ex)
        {
            result.Errors.Add(ex.Message);
        }
        return result;
    }
}