using System.Globalization;
using InvestGrain.App.Models;
using InvestGrain.Contracts.Models;

namespace InvestGrain.App.Views.Converters;

public class DesiredPriceVisibleConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not OrderTypeItem orderTypeItem) return false;
        return orderTypeItem.OrderType == OrderType.AtDesiredPrice;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool boolValue) return value!;
        return boolValue
            ? OrderTypeItem.AtDesiredPrice
            : OrderTypeItem.AtMarketPrice;
    }
}