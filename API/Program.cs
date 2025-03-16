using InvestGrain.API;
using InvestGrain.API.Repositories;
using InvestGrain.Contracts.Grains;
using InvestGrain.Contracts.Models;
using InvestGrain.ServiceDefaults;
using Microsoft.AspNetCore.Mvc;

const string corsPolicyName = "AllowAnyOrigin";

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.AddKeyedRedisClient("redis");
builder.UseOrleansClient();

builder.Services.ConfigureOpenAPI();
builder.Services.ConfigureCors(corsPolicyName);

var app = builder.Build();
app.UseOpenAPI();
app.UseCors(corsPolicyName);

app.MapGet("stocks", async (IClusterClient clusterClient)
    =>
{
    var stockNames = Stock.ListAll();
    var allStocks = new List<Stock>();
    foreach (var (symbol, _) in stockNames)
    {
        var stockGrain = clusterClient.GetGrain<IStockGrain>(symbol);
        var stock = await stockGrain.GetAsync();
        if (stock is null) continue;
        allStocks.Add(stock);
    }

    return Results.Json(allStocks);
});

app.MapGet("stocks/{symbol}", async (
        IClusterClient clusterClient,
        string symbol)
    =>
{
    var stockGrain = clusterClient.GetGrain<IStockGrain>(symbol);
    var stock = await stockGrain.GetAsync();
    return stock is null
        ? Results.NotFound($"{stock} not found")
        : Results.Json(stock);
});

app.MapGet("orders", (
        [FromServices] IOrdersRepository repository,
        [FromHeader(Name = "consumerId")] ulong consumerId)
    =>
{
    var orders = repository.ListByConsumerId(consumerId);
    return Results.Json(orders);
});

app.MapPost("orders/{symbol}/buy", async (
        [FromHeader(Name = "consumerId")] ulong consumerId,
        IClusterClient clusterClient,
        string symbol)
    =>
{
    var stockGrain = clusterClient.GetGrain<IStockGrain>(symbol);
    var stock = await stockGrain.GetAsync();
    if (stock is null) return Results.NotFound($"{stock} not found");

    var orderGrain = clusterClient.GetGrain<IOrderGrain>(Guid.NewGuid());
    var order = await orderGrain.SendAsync(consumerId, stock);

    return Results.Json(order);
});

app.MapPost("orders/{id}", async (
        [FromHeader(Name = "consumerId")] ulong consumerId,
        Guid id,
        IClusterClient clusterClient)
    =>
{
    var orderGrain = clusterClient.GetGrain<IOrderGrain>(id);
    var order = await orderGrain.GetAsync();

    return Results.Json(order);
});

app.Run();