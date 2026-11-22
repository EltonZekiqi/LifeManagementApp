using System.Globalization;
using Microsoft.Maui.Graphics;

namespace LifeManagementApp.Converters;

// Prioriteetti (int) -> väri listan reunapalkkiin
public class PriorityToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not int priority)
            return Colors.LightGray;

        return priority switch
        {
            >= 5 => Colors.Red,
            4 => Colors.OrangeRed,
            3 => Colors.Orange,
            2 => Colors.Gold,
            _ => Colors.LightGray
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
