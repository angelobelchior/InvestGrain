using InvestGrain.API;
using InvestGrain.Contracts.Grains;
using InvestGrain.ServiceDefaults;

const string corsPolicyName = "AllowAnyOrigin";

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.AddKeyedRedisClient("redis");
builder.UseOrleansClient();

builder.Services.ConfigureOpenAPI();
builder.Services.ConfigureCors(corsPolicyName);

var app = builder.Build();
app.UseOpenAPI();
app.UseCors(corsPolicyName);

app.MapGet("stocks/{stockName}", async (
        IClusterClient clusterClient,
        string stockName)
    =>
{
    var stockGrain = clusterClient.GetGrain<IStockGrain>(stockName);
    var stock = await stockGrain.GetAsync();
    return stock is null
        ? Results.NotFound()
        : Results.Json(stock);
});
app.Run();