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

app.MapGet("stocks", ()
    =>
{
    var allStocks = Stock.ListAll();
    return Results.Json(allStocks);
});

app.MapGet("stocks/{stockName}", async (
        IClusterClient clusterClient,
        string stockName)
    =>
{
    var stockGrain = clusterClient.GetGrain<IStockGrain>(stockName);
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

app.MapGet("orders/{stockName}/buy", async (
        [FromHeader(Name = "consumerId")] ulong consumerId,
        IClusterClient clusterClient,
        string stockName)
    =>
{
    var stockGrain = clusterClient.GetGrain<IStockGrain>(stockName);
    var stock = await stockGrain.GetAsync();
    if (stock is null) return Results.NotFound($"{stock} not found");

    var orderGrain = clusterClient.GetGrain<IOrderGrain>(Guid.NewGuid());
    var order = await orderGrain.SendAsync(consumerId, stock);

    return Results.Json(order);
});

app.Run();