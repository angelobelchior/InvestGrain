namespace InvestGrain.App.Services;

public class ServiceLocator
{
    private static ServiceLocator? _instance;
    private static readonly Lock _syncLock = new Lock();
    
    public static ServiceLocator Instance
    {
        get
        {
            lock (_syncLock)
            {
                return _instance ??= new ServiceLocator();
            }
        }
    }

    private StockService? _stockService;
    public StockService Stock => _stockService ??= new StockService();

    private ServiceLocator()
    {
    }
}