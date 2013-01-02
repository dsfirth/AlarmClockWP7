namespace AlarmClockWP7
{
    using System;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Contains all <see cref="Setting{T}"/> objects saved to persistent storage, which are controlled via the /SettingsPage.xaml.
    /// </summary>
    public class Settings
    {
        // Persistent user settings from the settings page
        public static readonly Setting<Color> ForegroundColor = new Setting<Color>("ForegroundColor", (Color)Application.Current.Resources["PhoneAccentColor"]);
        public static readonly Setting<Color> BackgroundColor = new Setting<Color>("BackgroundColor", Colors.Black);
        public static readonly Setting<bool> DisableScreenLock = new Setting<bool>("DisableScreenLock", true);
        public static readonly Setting<bool> Show24Hours = new Setting<bool>("Show24Hours", false);
        public static readonly Setting<bool> ShowSeconds = new Setting<bool>("ShowSeconds", true);
        public static readonly Setting<bool> EnableVibrations = new Setting<bool>("EnableVibrations", true);

        // Persistent user settings from the alarm page
        public static readonly Setting<DateTime> AlarmTime = new Setting<DateTime>("AlarmTime", new DateTime(1981, 11, 3, 8, 0, 0));
        public static readonly Setting<bool> IsAlarmOn = new Setting<bool>("IsAlarmOn", false);

        // Persistent state
        public static readonly Setting<DateTime?> SnoozeTime = new Setting<DateTime?>("SnoozeTime", null);
    }
}
