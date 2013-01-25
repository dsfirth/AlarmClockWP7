using System;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using AlarmClockWP7.Shared.Settings;
using AlarmClockWP7.ViewModels;
using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework.Audio; // For SoundEffectInstance

namespace AlarmClockWP7.Views
{

    public partial class AlarmPage
    {
        private readonly SoundEffectInstance _alarmSound;
        private readonly DispatcherTimer _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };

        /// <summary>
        /// Initializes a new instance of the <see cref="AlarmPage"/> class.
        /// </summary>
        public AlarmPage()
        {
            InitializeComponent();
            DataContext = new AlarmViewModel();
            _timer.Tick += Timer_Tick;

            _alarmSound = SoundEffects.Alarm.CreateInstance();
            _alarmSound.IsLooped = true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // vibrate for half a second
            VibrateController.Default.Start(TimeSpan.FromSeconds(.5));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // stop the vibration/sound effect if it's still playing
            _timer.Stop();
            _alarmSound.Stop();
        }

        private void ToggleSwitch_CheckedChanged(object sender, RoutedEventArgs e)
        {
            // if we're currently snoozing, cancel it
            UserSettings.SnoozeTime.Reset();
        }

        private void TimePicker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            // explicitly handle the ValueChanged event to prevent SnoozeTime being clobbered by OnNavigatedFrom
            UserSettings.SnoozeTime.Reset();
        }

        private void TestVolumeButton_Checked(object sender, RoutedEventArgs e)
        {
            // vibrate, only if it's enabled
            if (UserSettings.EnableVibration)
            {
                _timer.Start();
            }

            // play the sound
            _alarmSound.Play();
        }

        private void TestVolumeButton_Unchecked(object sender, RoutedEventArgs e)
        {
            // stop the sound and vibration
            _timer.Stop();
            _alarmSound.Stop();
        }
    }
}
