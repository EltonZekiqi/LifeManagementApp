using System.Globalization;

namespace LifeManagementApp.Converters;

// Teksti -> bool. Käytetään virheviestin näyttämiseen: näkyy vaan jos siinä on jotain
public class StringNotEmptyToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return !string.IsNullOrWhiteSpace(value as string);
    }

    // Toiseen suuntaan ei tarvita
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
