using System;
using System.Globalization;
using System.Windows.Data;
using AlarmClockWP7.Data;

namespace AlarmClockWP7.ViewModels
{
    public class BackgroundTypeToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var background = (BackgroundType)value;

            return background.ToString().ToLowerInvariant();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
