using InvestGrain.App.Models;
using InvestGrain.App.Services;
using InvestGrain.App.ViewModels;
using InvestGrain.Contracts.Models;

namespace InvestGrain.App.Views;

public partial class StocksPage
{
    public StocksPage()
    {
        InitializeComponent();

        var mainViewModel = new MainViewModel();
        BindingContext = mainViewModel;

        Appearing += async (_, _) => await mainViewModel.OnLoadingAsync();
        Disappearing += async (_, _) => await mainViewModel.OnUnloadingAsync();

        StocksCollectionView.SelectionChanged += async (_, _) =>
        {
            var stock = StocksCollectionView.SelectedItem as Stock;
            if (stock is null) return;

            var orderViewModel = new OrderViewModel(stock);
            await Navigation.PushAsync(new OrderPage(orderViewModel));
        };
    }
}