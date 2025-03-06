namespace InvestGrain.Contracts.Models;

[GenerateSerializer, Alias("InvestGrain.Contracts.Models.Order")]
public class Order
{
    public static readonly string CollectionName = "Orders";

    public static readonly string DatabasePath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "InvestGrain", "InvestGrain.db");

    [Id(0)] public Guid Id { get; set; }
    [Id(1)] public ulong ConsumerId { get; set; }
    [Id(2)] public Stock Stock { get; set; } = null!;
    [Id(3)] public OrderStatus Status { get; set; }
    [Id(4)] public DateTime DateTime { get; set; }

    public static Order Create(Guid id, ulong consumerId, Stock stock) => new Order
    {
        Id = id,
        ConsumerId = consumerId,
        Stock = stock,
        Status = OrderStatus.Pending,
        DateTime = DateTime.UtcNow
    };
}