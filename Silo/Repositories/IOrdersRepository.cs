using InvestGrain.Contracts.Models;
using LiteDB;

namespace InvestGrain.Silo.Repositories;

public interface IOrdersRepository
{
    void Insert(Order order);
    void Update(Order order);
}

internal class OrdersRepository : IOrdersRepository
{
    public void Insert(Order order)
    {
        using var database = new LiteDatabase(Order.DatabasePath);
        var collection = database.GetCollection<Order>(Order.CollectionName);
        collection.Insert(order);
        database.Commit();
    }

    public void Update(Order order)
    {
        using var database = new LiteDatabase(Order.DatabasePath);
        var collection = database.GetCollection<Order>(Order.CollectionName);
        collection.Update(order);
        database.Commit();
    }
}