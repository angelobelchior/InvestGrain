using InvestGrain.Contracts.Grains;
using InvestGrain.Contracts.Models;

namespace InvestGrain.Silo.Grains;

public class StockGrain(
    [PersistentState("stocksState", "stocks")] IPersistentState<Stock> stockState)
    : Grain, IStockGrain
{
    public async ValueTask UpdateAsync(Stock stock)
    {
        stockState.State = stock;
        await stockState.WriteStateAsync();
    }

    public override Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        if (reason.ReasonCode == DeactivationReasonCode.ShuttingDown)
            MigrateOnIdle();

        return base.OnDeactivateAsync(reason, cancellationToken);
    }
}