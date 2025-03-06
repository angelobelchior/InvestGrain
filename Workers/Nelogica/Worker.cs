using InvestGrain.Contracts.Grains;
using InvestGrain.Contracts.Models;
using InvestGrain.Worker.Nelogica.Repositories;

namespace InvestGrain.Worker.Nelogica;

public class Worker(IClusterClient client, IOrdersRepository repository) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var randomDelay = Random.Shared.Next(5000, 10000);
            await Task.Delay(randomDelay, stoppingToken);

            var orders = repository.ListByStatus(OrderStatus.Pending);

            foreach (var order in orders)
            {
                var historyGrain = client.GetGrain<IHistoryGrain>(order.Id);
                await historyGrain.UpdateAsync(OrderStatus.Completed);
            }
        }
    }
}