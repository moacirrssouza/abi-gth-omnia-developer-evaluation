namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

  /// <summary>
  /// Represents the command to update a sale item.
  /// </summary>
public class UpdateSaleItemCommand
{
    public Guid Id { get; set; }
    public string? Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public bool IsCancelled { get; set; }
}