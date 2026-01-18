using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handles the creation of sales by processing a CreateSaleCommand and returning the result.
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleCommandResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSaleHandler> _logger;
    private readonly IPublisher _publisher;

    /// <summary>
    /// Initializes a new instance of the CreateSaleHandler class with the specified dependencies.
    /// </summary>
    /// <param name="saleRepository">The repository used to persist and retrieve sale data.</param>
    /// <param name="mapper">The mapper used to convert between domain entities and data transfer objects.</param>
    /// <param name="logger">The logger used to record diagnostic and operational information for this handler.</param>
    /// <param name="publisher">The publisher used to dispatch events related to sales operations.</param>
    public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper, ILogger<CreateSaleHandler> logger, IPublisher publisher)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
        _publisher = publisher;
    }

    /// <summary>
    /// Handles the creation of a new sale based on the specified command.
    /// </summary>
    /// <remarks>This method maps the command to a sale entity, calculates discounts and totals, persists the
    /// sale, and publishes related domain events. If an error occurs during processing, the result will indicate
    /// failure and include error details.</remarks>
    /// <param name="command">The command containing the details required to create the sale. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a CreateSaleCommandResult indicating
    /// the outcome of the sale creation, including the sale identifier if successful and any errors if the operation
    /// fails.</returns>
    public async Task<CreateSaleCommandResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        CreateSaleCommandResult result = new();

        try
        {
            var sale = _mapper.Map<Sale>(command);
            sale.CalculateDiscountAndTotal();
            sale.AddEvent(new SaleCreatedEvent(sale));
            
            await _saleRepository.CreateAsync(sale, cancellationToken);
            
            foreach (var @event in sale.DomainEvents)
            {
                await _publisher.Publish(@event, cancellationToken);
            }

            result.Id = sale.Id;
            result.Success = true;
            _logger.LogInformation("SaleCreated: {SaleId}", sale.Id);
        }
        catch (Exception ex)
        {
            result.Errors.Add(ex.Message);
            result.Success = false;
        }
        return result;
    }
}
