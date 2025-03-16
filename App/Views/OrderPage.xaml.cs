using InvestGrain.App.ViewModels;

namespace InvestGrain.App.Views;

public partial class OrderPage
{
    public OrderPage(OrderViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;

        Appearing += async (_, _) => await viewModel.OnLoadingAsync();
        Disappearing += async (_, _) => await viewModel.OnUnloadingAsync();

        stockGrid.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = viewModel.LoadStockBySymbolCommand
        });
    }
}