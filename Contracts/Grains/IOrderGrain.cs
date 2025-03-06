using InvestGrain.Contracts.Models;

namespace InvestGrain.Contracts.Grains;

[Alias("InvestGrain.Contracts.Grains.IOrderGrain")]
public interface IOrderGrain : IGrainWithGuidKey
{
    [Alias("SendAsync")]
    Task<Order> SendAsync(ulong consumerId, Stock stock);
    
    [Alias("GetAsync")]
    Task<Order?> GetAsync();
}