using InvestGrain.Contracts.Grains;
using InvestGrain.Contracts.Models;

namespace InvestGrain.Worker.Nelogica;

public class MarketDataWorker(IClusterClient client)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var stocks = CreateStocks();
                var chunkedStocks = stocks.Chunk(stocks.Count / Environment.ProcessorCount);

                foreach (var chunkedStock in chunkedStocks)
                {
                    await Parallel.ForEachAsync(chunkedStock, parallelOptions: new ParallelOptions()
                    {
                        CancellationToken = stoppingToken,
                        MaxDegreeOfParallelism = Environment.ProcessorCount / 2,
                        TaskScheduler = TaskScheduler.Default
                    }, async (stock, _) =>
                    {
                        var grain = client.GetGrain<IStockGrain>(stock.Symbol);
                        await grain.UpdateValueAsync(stock);
                    });
                }

                // Regula o "throughput"...
                await Task.Delay(Random.Shared.Next(250), stoppingToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    private static IReadOnlyCollection<Stock> CreateStocks()
    {
        var stockNames = Stock.ListAll().OrderBy(_ => Guid.NewGuid());
        var stocks = new List<Stock>();

        foreach (var (symbol, name) in stockNames)
        {
            stocks.Add(new Stock
            {
                Name = name,
                Symbol = symbol,
                Icon = $"https://icons.brapi.dev/icons/{symbol}.svg",
                DateTime = DateTime.Now,
                Value = (decimal)(Random.Shared.NextDouble() * 100),
            });
        }

        return stocks;
    }
}