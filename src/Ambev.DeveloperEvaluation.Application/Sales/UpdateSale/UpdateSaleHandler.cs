using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleCommandResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateSaleHandler> _logger;
    private readonly IPublisher _publisher;

    public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper, ILogger<UpdateSaleHandler> logger, IPublisher publisher)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
        _publisher = publisher;
    }

    public async Task<UpdateSaleCommandResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        UpdateSaleCommandResult result = new();

        try
        {
            var existent = await _saleRepository.GetByIdAsync(command.Id);
            var wasCancelled = existent?.IsCancelled ?? false;

            if (existent == null)
            {
                throw new Exception("Resource Not Found");
            }

            var updatedSale = _mapper.Map<Sale>(command);
            updatedSale.CalculateDiscountAndTotal();

            updatedSale.AddEvent(new SaleModifiedEvent(updatedSale));

            if (!wasCancelled && updatedSale.IsCancelled)
            {
                updatedSale.AddEvent(new SaleCancelledEvent(updatedSale));
            }

            if (updatedSale.Items != null)
            {
                foreach (var item in updatedSale.Items.Where(i => i.IsCancelled))
                {
                    updatedSale.AddEvent(new ItemCancelledEvent(item, updatedSale));
                }
            }

            await _saleRepository.UpdateAsync(updatedSale, cancellationToken);
            result.Id = updatedSale.Id;
            result.Success = true;

            foreach (var @event in updatedSale.DomainEvents)
            {
                await _publisher.Publish(@event, cancellationToken);
            }

            updatedSale.ClearEvents();
        }
        catch (Exception ex)
        {
            result.Errors.Add(ex.Message);
        }
        return result;
    }
}
