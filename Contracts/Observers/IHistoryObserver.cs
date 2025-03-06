using InvestGrain.Contracts.Models;

namespace InvestGrain.Contracts.Observers;

[Alias("InvestGrain.Contracts.Grains.IHistoryObserver")]
public interface IHistoryObserver : IGrainObserver
{
    [Alias("OnOrderUpdatedAsync")]
    Task OnOrderUpdatedAsync(OrderStatus status);
}