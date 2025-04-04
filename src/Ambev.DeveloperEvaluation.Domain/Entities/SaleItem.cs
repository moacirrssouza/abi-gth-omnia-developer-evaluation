﻿using Ambev.DeveloperEvaluation.Domain.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale item.
/// </summary>
public class SaleItem : BaseEntity
{
	
	/// <summary>
	/// The product's unique identifier.
	/// </summary>
	public Guid ProductId { get; private set; }

	/// <summary>
	/// Represents the unique identifier for a sale. It is of type Guid, ensuring a globally unique value.
	/// </summary>
	public Guid SaleId { get; set; }

	/// <summary>
	/// The quantity of the product.
	/// </summary>
	public int Quantity { get; private set; }

	/// <summary>
	/// The unit price of the product.
	/// </summary>
	public decimal UnitPrice { get; private set; }

	/// <summary>
	/// The discount amount.
	/// </summary>
	public decimal Discount { get; private set; }


	/// <summary>
	/// The total amount of the item.
	/// </summary>
	public decimal TotalItemAmount => (UnitPrice * Quantity) - Discount;

	/// <summary>
	/// Indicates whether an operation has been cancelled. Defaults to false.
	/// </summary>
	public bool IsCancelled { get; set; } = true;

	[JsonIgnore]
	[ValidateNever]
	public Sale? Sale { get; set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="SaleItem"/> class.
	/// </summary>
	/// <param name="productId"></param>
	/// <param name="quantity"></param>
	/// <param name="unitPrice"></param>
	public SaleItem(Guid productId, int quantity, decimal unitPrice)
	{
		ProductId = productId;
		Quantity = quantity;
		UnitPrice = unitPrice;
		Discount = CalculateDiscount(quantity, unitPrice);
	}

	/// <summary>
	/// Cancels the saleItem.
	/// </summary>
	public void Cancel()
	{
		IsCancelled = true;
	}
	
	/// <summary>
	/// Calculates the discount amount.
	/// </summary>
	/// <param name="quantity"></param>
	/// <param name="unitPrice"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	private decimal CalculateDiscount(int quantity, decimal unitPrice)
	{
		if (quantity < 4) return 0;
		if (quantity >= 4 && quantity < 10) return unitPrice * quantity * 0.10m;
		if (quantity >= 10 && quantity <= 20) return unitPrice * quantity * 0.20m;
		throw new ArgumentException("Cannot sell more than 20 identical items.");
	}
}