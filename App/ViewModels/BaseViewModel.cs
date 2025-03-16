using CommunityToolkit.Mvvm.ComponentModel;

namespace InvestGrain.App.ViewModels;

public abstract class BaseViewModel : ObservableObject
{
    private bool _isLoaded;
    private bool _isUnloaded;

    public async Task OnLoadingAsync()
    {
        if (_isLoaded) return;
        await OnViewLoadingAsync();
        _isLoaded = true;
    }

    public async Task OnUnloadingAsync()
    {
        if (_isUnloaded) return;
        await OnViewUnloadingAsync();
        _isUnloaded = true;
    }
    
    protected virtual Task OnViewLoadingAsync()
        => Task.CompletedTask;

    protected virtual Task OnViewUnloadingAsync()
        => Task.CompletedTask;
}