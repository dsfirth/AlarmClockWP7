﻿namespace AlarmClockWP7
{
    using System;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Navigation;

    /// <summary>
    /// Code-behind class for /SettingsPage.xaml; page for selecting the AlarmClockWP7's application settings.
    /// </summary>
    public partial class SettingsPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsPage"/> class.
        /// </summary>
        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // respect saved settings
            ForegroundColorRectangle.Fill = new SolidColorBrush(Settings.ForegroundColor.Value);
            BackgroundColorRectangle.Fill = new SolidColorBrush(Settings.BackgroundColor.Value);
            DisableScreenLockToggleSwitch.IsChecked = Settings.DisableScreenLock.Value;
            Show24HourToggleSwitch.IsChecked = Settings.Show24Hours.Value;
            ShowSecondsToggleSwitch.IsChecked = Settings.ShowSeconds.Value;
            EnableVibrationToggleSwitch.IsChecked = Settings.EnableVibrations.Value;
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

        private void ForegroundColorRectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PickColor(Settings.ForegroundColor);
        }

        private void BackgroundColorRectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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