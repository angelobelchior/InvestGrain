using InvestGrain.Contracts.Grains;
using InvestGrain.Contracts.Models;

namespace InvestGrain.Worker.MarketData;

public class Worker(IClusterClient client)
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
                    }, async (stock, token) =>
                    {
                        var grain = client.GetGrain<IStockGrain>(stock.Name);
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
        var stockNames = Stock.ListAll();
        var stocks = new List<Stock>();

        foreach (var stockName in stockNames)
        {
            stocks.Add(new Stock
            {
                Name = stockName,
                DateTime = DateTime.Now,
                Value = (decimal)(Random.Shared.NextDouble() * 100),
            });
        }

        return stocks;
    }
}