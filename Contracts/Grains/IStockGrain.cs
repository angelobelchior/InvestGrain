using InvestGrain.Contracts.Models;

namespace InvestGrain.Contracts.Grains;

[Alias("InvestGrain.Contracts.Grains.IStockGrain")]
public interface IStockGrain : IGrainWithStringKey
{
    [Alias("UpdateValueAsync")]
    Task UpdateValueAsync(Stock stock);

    [Alias("GetAsync")]
    Task<Stock?> GetAsync();
}