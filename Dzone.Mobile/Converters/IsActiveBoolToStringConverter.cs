using System.Globalization;

namespace Dzone.Mobile.Converters
{
    public class IsActiveBoolToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isActive)
            {
                if (isActive is true)
                    return "متوفر";
            }

            return " غير متوفر";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}