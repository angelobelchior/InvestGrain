using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InvestGrain.App.Models;
using InvestGrain.App.Services;
using InvestGrain.Contracts.Models;

namespace InvestGrain.App.ViewModels;

public partial class OrderViewModel(
    Stock stock) : BaseViewModel
{
    public List<OrderTypeItem> OrderTypes { get; set; } =
    [
        OrderTypeItem.AtMarketPrice,
        OrderTypeItem.AtDesiredPrice,
    ];

    [ObservableProperty] private Stock? _stock;
    [ObservableProperty] private OrderTypeItem _orderTypeItem = OrderTypeItem.AtMarketPrice;
    [ObservableProperty] private int _stockQuantity;
    [ObservableProperty] private decimal _desiredPrice;
    [ObservableProperty] private double _availableBalance;

    protected override Task OnViewLoadingAsync()
        => LoadStockBySymbolAsync();

    [RelayCommand]
    private Task BuyAsync()
    {
        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task LoadStockBySymbolAsync()
        => Stock = await ServiceLocator.Instance.Stock.GetStockBySymbolAsync(stock.Symbol);
}