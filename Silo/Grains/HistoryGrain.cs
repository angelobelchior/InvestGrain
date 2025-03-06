using InvestGrain.Contracts.Grains;
using InvestGrain.Contracts.Models;
using InvestGrain.Contracts.Observers;

namespace InvestGrain.Silo.Grains;

public class HistoryGrain
    : Grain, IHistoryGrain
{
    private readonly HashSet<IHistoryObserver> _observers = new();
    
    public async Task UpdateAsync(OrderStatus status)
    {
        foreach (var observer in _observers)
            await observer.OnOrderUpdatedAsync(status);
    }

    public void Subscribe(IHistoryObserver observer)
        => _observers.Add(observer);

    public void Unsubscribe(IHistoryObserver observer)
        => _observers.Remove(observer);
}