using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Represents the command to update a sale.
/// </summary>
public class UpdateSaleCommand : IRequest<UpdateSaleCommandResult>
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string? Customer { get; set; }
    public string? Branch { get; set; }
    public List<UpdateSaleItemCommand>? Items { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsCancelled { get; set; }
}