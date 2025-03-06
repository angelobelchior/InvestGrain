using InvestGrain.Contracts.Models;
using LiteDB;

namespace InvestGrain.Worker.Nelogica.Repositories;

public interface IOrdersRepository
{
    IReadOnlyCollection<Order> ListByStatus(OrderStatus status);
}

internal class OrdersRepository : IOrdersRepository
{
    public IReadOnlyCollection<Order> ListByStatus(OrderStatus status)
    {
        using var database = new LiteDatabase(Order.DatabasePath);
        var collection = database.GetCollection<Order>(Order.CollectionName);
        var orders = collection.Query().Where(o => o.Status == status)?.ToList();
        return orders ?? [];
    }
}