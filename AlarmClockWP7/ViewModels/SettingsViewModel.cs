using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using AlarmClockWP7.Annotations;
using AlarmClockWP7.Data;
using AlarmClockWP7.Shared.Settings;

namespace AlarmClockWP7.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private static readonly AccentColorType[] AccentColorsInternal =
            {
                AccentColorType.Default,
                AccentColorType.Magenta,
                AccentColorType.Purple,
                AccentColorType.Teal,
                AccentColorType.Lime,
                AccentColorType.Brown,
                AccentColorType.Pink,
                AccentColorType.Mango,
                AccentColorType.Blue,
                AccentColorType.Red,
                AccentColorType.Green
            };

        public IEnumerable<AccentColorType> AccentColors
        {
            get { return new ReadOnlyCollection<AccentColorType>(AccentColorsInternal); }
        }

        public AccentColorType SelectedAccentColor
        {
            get { return UserSettings.AccentColor; }
            set
            {
                UserSettings.AccentColor.TrySet(value);
                OnPropertyChanged("SelectedAccentColor");
            }
        }

        private static readonly BackgroundType[] BackgroundsInternal =
            {
                BackgroundType.Auto,
                BackgroundType.Dark,
                BackgroundType.Light
            };

        public IEnumerable<BackgroundType> Backgrounds
        {
            get { return new ReadOnlyCollection<BackgroundType>(BackgroundsInternal); }
        }

        public BackgroundType SelectedBackground
        {
            get { return UserSettings.Background; }
            set
            {
                UserSettings.Background.TrySet(value);
                OnPropertyChanged("SelectedBackground");
            }
        }

        public bool DisableScreenLock
        {
            get { return UserSettings.DisableScreenLock; }
            set
            {
                UserSettings.DisableScreenLock.TrySet(value);
                OnPropertyChanged("DisableScreenLock");
            }
        }

        public bool Show24HourTime
        {
            get { return UserSettings.Show24HourTime; }
            set
            {
                UserSettings.Show24HourTime.TrySet(value);
                OnPropertyChanged("Show24HourTime");
            }
        }

        public bool ShowSeconds
        {
            get { return UserSettings.ShowSeconds; }
            set
            {
                UserSettings.ShowSeconds.TrySet(value);
                OnPropertyChanged("ShowSeconds");
            }
        }

        public bool EnableVibration
        {
            get { return UserSettings.EnableVibration; }
            set
            {
                UserSettings.EnableVibration.TrySet(value);
                OnPropertyChanged("EnableVibration");
            }
        }

        #region - INotifyPropertyChanged implementation -

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
