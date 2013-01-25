using System;
using System.Globalization;
using System.Windows.Data;
using AlarmClockWP7.Data;

namespace AlarmClockWP7.ViewModels
{
    public class AccentColorTypeToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var accentColor = (AccentColorType)value;

            return accentColor.ToString().ToLowerInvariant();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
