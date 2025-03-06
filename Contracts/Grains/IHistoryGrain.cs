using InvestGrain.Contracts.Models;
using InvestGrain.Contracts.Observers;

namespace InvestGrain.Contracts.Grains;

[Alias("InvestGrain.Contracts.Grains.IHistoryGrain")]
public interface IHistoryGrain : IGrainWithGuidKey
{
    [Alias("UpdateAsync")]
    Task UpdateAsync(OrderStatus status);
    
    [Alias("Subscribe")]
    void Subscribe(IHistoryObserver observer);
    
    [Alias("Unsubscribe")]
    void Unsubscribe(IHistoryObserver observer);
}