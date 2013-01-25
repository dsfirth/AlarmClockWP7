using System;
using System.Globalization;
using System.Windows.Data;
using AlarmClockWP7.Data;

namespace AlarmClockWP7.ViewModels
{
    public class AccentColorTypeToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var accentColor = (AccentColorType)value;

            return ((uint)accentColor).ToSolidColorBrush();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
