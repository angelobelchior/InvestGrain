using InvestGrain.Contracts.Grains;
using InvestGrain.Contracts.Models;
using InvestGrain.Contracts.Observers;
using InvestGrain.Silo.Repositories;

namespace InvestGrain.Silo.Grains;

public class OrderGrain(
    IOrdersRepository repository,
    [PersistentState("ordersValue", "orders")]
    IPersistentState<Order> data)
    : Grain, IOrderGrain, IHistoryObserver
{
    public async Task<Order> SendAsync(ulong consumerId, Stock stock)
    {
        var id = this.GetPrimaryKey();
        data.State = Order.Create(id, consumerId, stock);
        await data.WriteStateAsync();

        repository.Insert(data.State);

        SubscribeToListenHistory();

        return data.State;
    }

    public Task<Order?> GetAsync()
    {
        var order = !data.RecordExists
            ? null
            : data.State;

        return Task.FromResult(order);
    }

    public async Task OnOrderUpdatedAsync(OrderStatus status)
    {
        data.State.Status = status;
        await data.WriteStateAsync();

        repository.Update(data.State);
    }

    public override Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        UnsubscribeToListenHistory();
        return base.OnDeactivateAsync(reason, cancellationToken);
    }

    private void SubscribeToListenHistory()
    {
        var id = this.GetPrimaryKey();
        var historyGrain = GrainFactory.GetGrain<IHistoryGrain>(id);
        historyGrain.Subscribe(this.AsReference<IHistoryObserver>());
    }

    private void UnsubscribeToListenHistory()
    {
        var id = this.GetPrimaryKey();
        var historyGrain = GrainFactory.GetGrain<IHistoryGrain>(id);
        historyGrain.Unsubscribe(this.AsReference<IHistoryObserver>());
    }
}