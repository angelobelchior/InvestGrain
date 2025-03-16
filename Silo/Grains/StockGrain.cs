using InvestGrain.Contracts.Grains;
using InvestGrain.Contracts.Models;

namespace InvestGrain.Silo.Grains;

public class StockGrain(
    [PersistentState("stockValue", "stocks")]
    IPersistentState<Stock> data)
    : Grain, IStockGrain
{
    public async Task UpdateValueAsync(Stock stock)
    {
        if (data.RecordExists)
        {
            var change = stock.Value - data.State.Value;
            stock.Change = change;
        }

        data.State = stock;
        await data.WriteStateAsync();
    }

    public Task<Stock?> GetAsync()
    {
        var stock = !data.RecordExists
            ? null
            : data.State;

        return Task.FromResult(stock);
    }

    public override Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        if (reason.ReasonCode == DeactivationReasonCode.ShuttingDown)
            MigrateOnIdle();

        return base.OnDeactivateAsync(reason, cancellationToken);
    }
}