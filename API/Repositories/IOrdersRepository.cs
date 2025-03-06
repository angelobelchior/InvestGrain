using InvestGrain.Contracts.Models;
using LiteDB;

namespace InvestGrain.API.Repositories;

public interface IOrdersRepository
{
    IReadOnlyCollection<Order> ListByConsumerId(ulong consumerId);
}

internal class OrdersRepository : IOrdersRepository
{
    public IReadOnlyCollection<Order> ListByConsumerId(ulong consumerId)
    {
        using var database = new LiteDatabase(Order.DatabasePath);
        var collection = database.GetCollection<Order>(Order.CollectionName);
        var orders = collection.Query().Where(o => o.ConsumerId == consumerId)?.ToList();
        return orders ?? [];
    }
}