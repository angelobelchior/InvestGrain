using InvestGrain.Contracts.Models;

namespace InvestGrain.UI;

public class APIClient(HttpClient client)
{
    public async Task<string[]> ListAllStocksAsync()
    {
        var response = await client.GetAsync("stocks");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<string[]>() ?? [];
    }

    public async Task<Stock?> GetStockByNameAsync(string stockName)
    {
        var response = await client.GetAsync($"stocks/{stockName}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Stock>();
    }

    public async Task<Order?> BuyAsync(ulong consumerId, string stockName)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"orders/{stockName}/buy");
        request.Headers.Add("consumerId", consumerId.ToString());
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Order>();
    }
    
    public async Task<Order?> GetOrderByIdAsync(Guid orderId)
    {
        var response = await client.GetAsync($"orders/{orderId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Order>();
    }

    public async Task<IReadOnlyCollection<Order>> GetHistoryAsync(ulong consumerId)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "orders");
        request.Headers.Add("consumerId", consumerId.ToString());
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var orders = await response.Content.ReadFromJsonAsync<Order[]>();
        return orders ?? [];
    }
}