using InvestGrain.Contracts.Models;

namespace InvestGrain.App.Models;

public record OrderTypeItem(OrderType OrderType, string Description)
{
    public static readonly OrderTypeItem AtMarketPrice = new(OrderType.AtMarketPrice, "Pelo valor de Mercado");
    public static readonly OrderTypeItem AtDesiredPrice = new(OrderType.AtDesiredPrice, "Pelo Valor Desejado");

    public override string ToString() => Description;
}