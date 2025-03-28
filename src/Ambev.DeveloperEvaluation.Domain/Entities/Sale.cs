﻿using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
	/// <summary>
	/// Represents a sale.
	/// </summary>
	public class Sale : BaseEntity
	{
		/// <summary>
		/// The total amount of the sale.
		/// </summary>
		private decimal _totalAmount;

		/// <summary>
		/// The date and time the sale was made.
		/// </summary>
		public DateTime SaleDate { get; set; } = DateTime.UtcNow;

		/// <summary>
		/// The customer's unique identifier.
		/// </summary>
		public Guid CustomerId { get; set; }

		/// <summary>
		/// The branch's unique identifier.
		/// </summary>
		public Guid BranchId { get; set; }

		/// <summary>
		/// The total amount of the sale.
		/// </summary>
		public bool IsCancelled { get; set; } = false;

		/// <summary>
		/// The total amount of the sale.
		/// </summary>
		public decimal TotalAmount
		{
			get => _totalAmount;
			private set => _totalAmount = value;
		}

		private Sale() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="Sale"/> class.
		/// </summary>
		/// <param name="customerId"></param>
		/// <param name="branchId"></param>
		public Sale(Guid customerId, Guid branchId)
		{
			CustomerId = customerId;
			BranchId = branchId;
			RecalculateTotal();
		}

		/// <summary>
		/// Adds an item to the sale.
		/// </summary>
		/// <param name="item"></param>
		public void AddItem()
		{
			RecalculateTotal();
		}

		/// <summary>
		/// Cancels the sale.
		/// </summary>
		public void Cancel()
		{
			IsCancelled = true;
		}

		/// <summary>
		/// Recalculates the total amount of the sale.
		/// </summary>
		public void RecalculateTotal()
		{
			TotalAmount = _totalAmount;
		}
	}
}