using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Common.Events;
using MediatR;


namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
	/// <summary>
	/// Handles the cancellation of a sale by interacting with the sale repository and publishing an event upon success.
	/// Returns a response indicating the success of the cancellation.
	/// </summary>
	public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResponse>
	{
		private readonly ISaleRepository _saleRepository;
		private readonly IEventPublisher _eventPublisher;

		public CancelSaleHandler(ISaleRepository saleRepository, IEventPublisher eventPublisher)
		{
			_saleRepository = saleRepository;
			_eventPublisher = eventPublisher;
		}

		/// <summary>
		/// Handles the cancellation of a sale by processing a command and publishing an event if successful.
		/// </summary>
		/// <param name="command">Contains the details necessary to identify and cancel the specific sale.</param>
		/// <param name="cancellationToken">Used to signal the operation to cancel if needed.</param>
		/// <returns>Returns a response indicating the success or failure of the cancellation operation.</returns>
		public async Task<CancelSaleResponse> Handle(CancelSaleCommand command, CancellationToken cancellationToken)
		{
			bool success = await _saleRepository.CancelSaleAsync(command.SaleId, cancellationToken);
			if (success)
			{
				var saleCancelledEvent = new SaleCancelledEvent(command.SaleId);
				await _eventPublisher.PublishAsync(saleCancelledEvent);
				return new CancelSaleResponse { Success = true, Message = "Sale cancelled successfully." };
			}
			else
			{
				return new CancelSaleResponse { Success = false, Message = "Sale not found or could not be cancelled." };
			}
		}
	}
}
