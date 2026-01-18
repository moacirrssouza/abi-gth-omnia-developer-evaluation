using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Sale Domain Entity
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    /// Sale Date
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Customer Name
    /// </summary>
    public string Customer { get; set; } = string.Empty;

    /// <summary>
    /// Branch Text
    /// </summary>
    public string Branch { get; set; } = string.Empty;

    /// <summary>
    /// Sales Items
    /// </summary>
    public List<SaleItem> Items { get; set; } = new();

    /// <summary>
    /// Sale Amount Value
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Cancellation Flag
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Sale"/> class.
    /// </summary>
    public Sale()
    {
        Date = DateTime.UtcNow;
    }
    /// <summary>
    /// Cancels the sale.
    /// </summary>
    public void Cancel()
    {
        if (IsCancelled) return;
        IsCancelled = true;
        AddEvent(new SaleCancelledEvent(this));
    }

    /// <summary>
    /// Updates the total amount of the sale.
    /// </summary>
    public void UpdateTotalAmount()
    {
        TotalAmount = Items.Sum(i => i.TotalAmount);
    }

    /// <summary>
    /// Calculates the discount and total amount of the sale items.
    /// </summary>
    /// <exception cref="DomainException">Thrown when the quantity of any item exceeds 20 or when the quantity of any item is negative.</exception>
    public void CalculateDiscountAndTotal()
    {
        if (Items == null || Items.Count == 0)
        {
            TotalAmount = 0;
            return;
        }

        var hasItemAboveLimit = Items
            .GroupBy(x => x.Product)
            .Any(group => group.Sum(x => x.Quantity) > 20);

        if (hasItemAboveLimit)
        {
            throw new DomainException("It's not possible to sell above 20 identical items.");
        }

        foreach (var item in Items)
        {
            if (item.Quantity < 0)
                throw new DomainException("Item quantity cannot be negative.");

            if (item.Quantity == 0)
            {
                item.Discount = 0;
                continue;
            }

            var gross = item.Quantity * item.UnitPrice;
            decimal discountPercentage = 0;

            if (item.Quantity >= 4 && item.Quantity < 10)
            {
                discountPercentage = 0.10m;
            }
            else if (item.Quantity >= 10 && item.Quantity <= 20)
            {
                discountPercentage = 0.20m;
            }

            item.Discount = gross * discountPercentage;
        }

        TotalAmount = Items.Sum(i => (i.Quantity * i.UnitPrice) - i.Discount);
    }
}