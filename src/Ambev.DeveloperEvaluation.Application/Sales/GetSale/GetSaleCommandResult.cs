using Ambev.DeveloperEvaluation.Application.Base;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
///    Represents the result of a get sale command.
/// </summary>
public class GetSaleCommandResult : BaseResult
{
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    public string? Customer { get; set; }

    public string? Branch { get; set; }

    public List<GetSaleItemCommandResult>? Items { get; set; }

    public decimal TotalAmount { get; set; }

    public bool IsCancelled { get; set; }

}

/// <summary>
///  Represents the result of a get sale item command.
/// </summary>
public class GetSaleItemCommandResult
{
    public Guid Id { get; set; }

    public string? Product { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Discount { get; set; }

    public bool IsCancelled { get; set; }
}