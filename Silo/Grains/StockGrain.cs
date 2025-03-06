using InvestGrain.Contracts.Grains;
using InvestGrain.Contracts.Models;

namespace InvestGrain.Silo.Grains;

public class StockGrain(
    [PersistentState("stocksState", "stocks")]
    IPersistentState<Stock> stockState)
    : Grain, IStockGrain
{
    public async Task UpdateAsync(Stock stock)
    {
        stockState.State = stock;
        await stockState.WriteStateAsync();
    }

    public Task<Stock?> GetAsync()
    {
        var stock = !stockState.RecordExists
            ? null 
            : stockState.State;
        
        return Task.FromResult(stock);
    }

    public override Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        if (reason.ReasonCode == DeactivationReasonCode.ShuttingDown)
            MigrateOnIdle();

        return base.OnDeactivateAsync(reason, cancellationToken);
    }
}