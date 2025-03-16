using System.Globalization;

namespace InvestGrain.App.Views.Converters;

public class ChangeColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double change)
            return change >= 0 ? Colors.Green : Colors.Red;
        return Colors.Black;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value;
}