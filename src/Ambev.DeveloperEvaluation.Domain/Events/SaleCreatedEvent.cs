﻿namespace Ambev.DeveloperEvaluation.Domain.Events
{
	public class SaleCreatedEvent : SaleEvent
	{
		public Guid SaleId { get; }
		public SaleCreatedEvent(Guid saleId)
		{
			SaleId = saleId;
		}
	}
}
