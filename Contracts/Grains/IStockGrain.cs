using InvestGrain.Contracts.Models;

namespace InvestGrain.Contracts.Grains;

[Alias("InvestGrain.Contracts.Grains.IStockGrain")]
public interface IStockGrain : IGrainWithStringKey
{
    [Alias("UpdateAsync")]
    ValueTask UpdateAsync(Stock stock);
}