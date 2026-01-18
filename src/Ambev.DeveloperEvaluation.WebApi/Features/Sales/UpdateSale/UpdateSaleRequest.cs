namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Request to update a sale.
/// </summary>
public class UpdateSaleRequest
{
    public DateTime Date { get; set; }
    public string Customer { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
    public List<UpdateSaleItemRequest> Items { get; set; } = [];
    public bool IsCancelled { get; set; }
}