using System;
using AlarmClockWP7.Data;
using AlarmClockWP7.Shared.Settings;
using Microsoft.Phone.Controls;

namespace AlarmClockWP7
{
    /// <summary>
    /// Contains all <see cref="Setting{T}"/> objects saved to persistent storage, which are controlled via the /Views/SettingsPage.xaml.
    /// </summary>
    public class UserSettings
    {
        // persistent user-settings from /View/SettingsPage.xaml
        public static readonly Setting<BackgroundType> Background = new Setting<BackgroundType>("Background");
        public static readonly Setting<AccentColorType> AccentColor = new Setting<AccentColorType>("AccentColor");

        public static readonly Setting<bool> DisableScreenLock = new Setting<bool>("DisableScreenLock", true);
        public static readonly Setting<bool> Show24HourTime = new Setting<bool>("Show24HourTime");
        public static readonly Setting<bool> ShowSeconds = new Setting<bool>("ShowSeconds", true);
        public static readonly Setting<bool> EnableVibration = new Setting<bool>("EnableVibration", true);

        // persistent user-settings from /View/AlarmPage.xaml
        public static readonly Setting<DateTime> AlarmTime =
            new Setting<DateTime>("AlarmTime", new DateTime(1981, 11, 3, 8, 0, 0));

        public static readonly Setting<bool> IsAlarmOn = new Setting<bool>("IsAlarmOn");

        // Persistent state
        public static readonly Setting<SupportedPageOrientation> SupportedOrientations =
            new Setting<SupportedPageOrientation>("SupportedOrientations", SupportedPageOrientation.PortraitOrLandscape);

        public static readonly Setting<DateTime?> SnoozeTime = new Setting<DateTime?>("SnoozeTime");
    }
}
