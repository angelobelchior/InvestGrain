using InvestGrain.Contracts.Models;
using LiteDB;

namespace InvestGrain.Worker.Nelogica.Repositories;

public interface IOrdersRepository
{
    IReadOnlyCollection<Order> ListAllNotCompleted();
}

internal class OrdersRepository : IOrdersRepository
{
    public IReadOnlyCollection<Order> ListAllNotCompleted()
    {
        using var database = new LiteDatabase(Order.DatabasePath);
        var collection = database.GetCollection<Order>(Order.CollectionName);
        var orders = collection.Query()
            .Where(o => o.Status != OrderStatus.Completed)?
            .ToList();
        return orders ?? [];
    }
}