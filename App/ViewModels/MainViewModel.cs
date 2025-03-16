using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using InvestGrain.App.Models;
using InvestGrain.App.Services;
using InvestGrain.Contracts.Models;

namespace InvestGrain.App.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    public ObservableCollection<Stock> Stocks { get; } = new();

    protected override Task OnViewLoadingAsync()
        => LoadStocksAsync();

    [RelayCommand]
    private Task RefreshAsync()
        => LoadStocksAsync();

    private async Task LoadStocksAsync()
    {
        Stocks.Clear();

        var stocks = await ServiceLocator.Instance.Stock.ListAllStocksAsync();
        foreach (var stock in stocks)
            Stocks.Add(stock);
    }
}