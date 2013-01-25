using System;
using System.Windows.Media;
using System.Windows.Navigation;

namespace AlarmClockWP7
{
    /// <summary>
    /// Code-behind class for /SettingsPage.xaml; page for selecting the AlarmClockWP7's application settings.
    /// </summary>
    public partial class SettingsPage
    {
        // ctor
        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // respect saved settings
            ForegroundColorButton.Background = new SolidColorBrush(Settings.ForegroundColor);
            BackgroundColorButton.Background = new SolidColorBrush(Settings.BackgroundColor);
            DisableScreenLockToggleSwitch.IsChecked = Settings.DisableScreenLock;
            Show24HourToggleSwitch.IsChecked = Settings.Show24Hours;
            ShowSecondsToggleSwitch.IsChecked = Settings.ShowSeconds;
            EnableVibrationToggleSwitch.IsChecked = Settings.EnableVibrations;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // save the settings (except the colours, already saved by the color picker)
            Settings.DisableScreenLock.TrySet(DisableScreenLockToggleSwitch.IsChecked);
            Settings.Show24Hours.TrySet(Show24HourToggleSwitch.IsChecked);
            Settings.ShowSeconds.TrySet(ShowSecondsToggleSwitch.IsChecked);
            Settings.EnableVibrations.TrySet(EnableVibrationToggleSwitch.IsChecked);
        }

        private void ForegroundColorButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PickColor(Settings.ForegroundColor);
        }

        private void BackgroundColorButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PickColor(Settings.BackgroundColor);
        }

        private void PickColor(Setting<Color> setting)
        {
            // get a string representation of the colours we need to pass to the colour picker,
            // without the leading '#' character
            string currentColorString = setting.Value.ToString().Substring(1),
                   defaultColorString = setting.Value.ToString().Substring(1);

            // the colour picker works with the same isolated storate value that the Setting works
            // with, but we have to clear its cached value to pick up the value chosen in the color
            // picker
            setting.ForceRefresh();

            // navigate to the /Shared/ColorPicker/ColorPicker.xaml
            string uriString =
                string.Format(
                    "/Shared/ColorPicker/ColorPicker.xaml?&currentColor={0}&defaultColor={1}&settingName={2}",
                    currentColorString,
                    defaultColorString,
                    setting.Key);
            NavigationService.Navigate(new Uri(uriString, UriKind.Relative));
        }
    }
}
