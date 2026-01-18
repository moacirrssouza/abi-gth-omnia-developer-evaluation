using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Sale Item Domain Entity
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// Product Description 
    /// </summary>
    public string Product { get; set; } = string.Empty;

    /// <summary>
    /// Item Quantity
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Item Unit Price
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Discount Amount When Applicable
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Cancellation flag
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// Total amount for this item (Quantity * UnitPrice - Discount)
    /// </summary>
    public decimal TotalAmount => (Quantity * UnitPrice) - Discount;

    public void Cancel()
    {
        IsCancelled = true;
    }
}