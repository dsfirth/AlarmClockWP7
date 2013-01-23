namespace AlarmClockWP7
{
    using System;
    using System.Windows;
    using System.Windows.Navigation;
    using System.Windows.Threading;
    using Microsoft.Devices;
    using Microsoft.Phone.Controls;
    using Microsoft.Xna.Framework.Audio;    // For SoundEffectInstance

    public partial class AlarmPage
    {
        private readonly SoundEffectInstance _alarmSound;
        private readonly DispatcherTimer _timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(1)};

        /// <summary>
        /// Initializes a new instance of the <see cref="AlarmPage"/> class.
        /// </summary>
        public AlarmPage()
        {
            InitializeComponent();
            _timer.Tick += Timer_Tick;

            _alarmSound = SoundEffects.Alarm.CreateInstance();
            _alarmSound.IsLooped = true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // vibrate for half a second
            VibrateController.Default.Start(TimeSpan.FromSeconds(.5));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // respect saved settings
            ToggleSwitch.IsChecked = Settings.IsAlarmOn.Value;
            TimePicker.Value = Settings.AlarmTime.Value;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // save the settings (except AlarmTime, handled in TimePicker_ValueChanged)
            Settings.IsAlarmOn.TrySet(ToggleSwitch.IsChecked);

            // stop the vibration/sound effect if it's still playing
            _timer.Stop();
            _alarmSound.Stop();
        }

        private void ToggleSwitch_CheckedChanged(object sender, RoutedEventArgs e)
        {
            // if we're currently snoozing, cencel it
            Settings.SnoozeTime.Reset();
        }

        private void TimePicker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            // explicitly handle the ValueChanged event to prevent SnoozeTime being clobbered by OnNavigatedFrom
            Settings.AlarmTime.TrySet(TimePicker.Value);
            Settings.SnoozeTime.Reset();
        }

        private void TestVolumeButton_Checked(object sender, RoutedEventArgs e)
        {
            // vibrate, only if it's enabled
            if (Settings.EnableVibrations.Value)
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
