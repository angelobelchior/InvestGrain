using System.Net.Http.Json;
using InvestGrain.Contracts.Models;
namespace InvestGrain.App.Services;

public class StockService
{
    
#if ANDROID
    private Uri _baseUrl = new("http://10.0.2.2:5371/");
#else
    private Uri _baseUrl = new("http://localhost:5371//");
#endif


    public async Task<Stock[]> ListAllStocksAsync()
    {
        using var httpClient = GetHttpClient();
        var response = await httpClient.GetAsync("stocks");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Stock[]>() ?? [];
    }

    public async Task<Stock?> GetStockBySymbolAsync(string symbol)
    {
        using var httpClient = GetHttpClient();
        var response = await httpClient.GetAsync($"stocks/{symbol}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Stock>();
    }

    public async Task<Order?> BuyAsync(ulong consumerId, string symbol)
    {
        using var httpClient = GetHttpClient();
        using var request = new HttpRequestMessage(HttpMethod.Post, $"orders/{symbol}/buy");
        request.Headers.Add("consumerId", consumerId.ToString());
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Order>();
    }

    public async Task<Order?> GetOrderByIdAsync(Guid orderId)
    {
        using var httpClient = GetHttpClient();
        var response = await httpClient.GetAsync($"orders/{orderId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Order>();
    }

    public async Task<IReadOnlyCollection<Order>> GetHistoryAsync(ulong consumerId)
    {
        using var httpClient = GetHttpClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, "orders");
        request.Headers.Add("consumerId", consumerId.ToString());
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var orders = await response.Content.ReadFromJsonAsync<Order[]>();
        return orders ?? [];
    }
    
    private HttpClient GetHttpClient()
    {
        var httpClient = new HttpClient();
        httpClient.BaseAddress = _baseUrl;
        return httpClient;
    }
}