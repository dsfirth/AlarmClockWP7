using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework.Audio;

namespace AlarmClockWP7
{
    public partial class MainPage
    {
        // DispatcherTimer: update the display every second.
        private readonly DispatcherTimer _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };

        private readonly TextBlock[] _dayOfWeekTextBlocks = new TextBlock[7];
        private static readonly string[] DayOfWeekText = new[] { "SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT" };
        private readonly SoundEffectInstance _alarmSound;
        private bool _tappedAlarmOff;

        // ctor
        public MainPage()
        {
            InitializeComponent();

            _timer.Tick += Timer_Tick;
            _timer.Start();

            // initialize the alarm sound effect
            SoundEffects.Initialize();
            _alarmSound = SoundEffects.Alarm.CreateInstance();
            _alarmSound.IsLooped = true;

            // add the day-of-week text blocks, tracking them in an array
            for (int i = 0; i < _dayOfWeekTextBlocks.Length; i++)
            {
                _dayOfWeekTextBlocks[i] = new TextBlock { Text = DayOfWeekText[i], Style = DayOfWeekStyle };
                Grid.SetColumn(_dayOfWeekTextBlocks[i], i + 1);

                LayoutRoot.Children.Add(_dayOfWeekTextBlocks[i]);
            }

            // allow app to run (making alarm sound/vibration) even when phone is locked
            // NOTE: once disabled, you cannot re-enable the default behaviour!
            PhoneApplicationService.Current.ApplicationIdleDetectionMode = IdleDetectionMode.Disabled;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // restore the ability for the screen to auto-lock when on other pages
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Enabled;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _tappedAlarmOff = false;

            // respect the saved settings
            Foreground = new SolidColorBrush(Settings.ForegroundColor);
            LayoutRoot.Background = new SolidColorBrush(Settings.BackgroundColor);
            ApplicationBar.ForegroundColor = Settings.ForegroundColor;
            ApplicationBar.BackgroundColor = Settings.BackgroundColor;

            // while on this page, don't allow the screen to auto-lock
            if (Settings.DisableScreenLock)
            {
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

            SupportedOrientations = Settings.SupportedOrientations;

            if (SupportedOrientations != SupportedPageOrientation.PortraitOrLandscape)
            {
                var orientationLockButton = (IApplicationBarIconButton)ApplicationBar.Buttons[2];

                orientationLockButton.Text = "unlock";
                orientationLockButton.IconUri = new Uri("/Images/appbar.orientationLocked.png", UriKind.Relative);
            }

            RefreshDisplay();

            // don't wait for the next tick
            Timer_Tick(this, EventArgs.Empty);
        }

        protected override void OnOrientationChanged(OrientationChangedEventArgs e)
        {
            base.OnOrientationChanged(e);
            RefreshDisplay();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (_alarmSound.State == SoundState.Playing)
            {
                // turn off the alarm
                _tappedAlarmOff = true;
                _alarmSound.Stop();

                // set the snooze time to 5 minutes from now
                DateTime currentTimeWithoutSeconds = DateTime.Now;
                currentTimeWithoutSeconds = currentTimeWithoutSeconds.AddSeconds(-currentTimeWithoutSeconds.Second);

                Settings.SnoozeTime.Value = currentTimeWithoutSeconds.AddMinutes(5);

                RefreshDisplay();
            }
            else
            {
                // toggle the application bar visibility
                ApplicationBar.IsVisible = !ApplicationBar.IsVisible;
            }
        }

        private void RefreshDisplay()
        {
            if (IsMatchingOrientation(PageOrientation.PortraitUp))
            {
                // adjust the margins for portrait
                LeftMargin.Width = RightMargin.Width = new GridLength(12);

                // set the font size accordingly
                CurrentTimeDisplay.FontSize = Settings.ShowSeconds ? 182 : 223;
            }
            else
            {
                // adjust the margins for landscape
                LeftMargin.Width = RightMargin.Width = new GridLength(92);

                // set the font size accordingly
                CurrentTimeDisplay.FontSize = Settings.ShowSeconds ? 251 : 307;
            }

            AlarmTimeDisplay.FontSize = CurrentTimeDisplay.FontSize/2;

            // respect the settings in the two time displays
            CurrentTimeDisplay.Show24Hours = AlarmTimeDisplay.Show24Hours = Settings.Show24Hours;
            CurrentTimeDisplay.ShowSeconds = Settings.ShowSeconds;
            CurrentTimeDisplay.Initialize();
            AlarmTimeDisplay.Initialize();

            if (Settings.IsAlarmOn)
            {
                if (Settings.SnoozeTime.IsSet)
                {
                    // indicate that we're in snooze
                    AlarmOnTextBlock.Opacity = .1;
                    SnoozeUntilTextBlock.Opacity = 1;
                    AlarmTimeDisplay.Time = Settings.SnoozeTime;
                }
                else
                {
                    // show when the alarm will sound
                    AlarmOnTextBlock.Opacity = 1;
                    SnoozeUntilTextBlock.Opacity = .1;
                    AlarmTimeDisplay.Time = Settings.AlarmTime;
                }
            }
            else
            {
                // no alarm; no snooze
                AlarmOnTextBlock.Opacity = .1;
                SnoozeUntilTextBlock.Opacity = .1;
                AlarmTimeDisplay.Time = null;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var current = DateTime.Now;

            // refresh the current time
            CurrentTimeDisplay.Time = current;

            // keep the day of the week up-to-date
            foreach (TextBlock textBlock in _dayOfWeekTextBlocks)
            {
                textBlock.Opacity = .2;
            }
            _dayOfWeekTextBlocks[(int)current.DayOfWeek].Opacity = 1;

            // if the alarm sound is playing, accompany it with vibration (if enabled)
            if (_alarmSound.State == SoundState.Playing && Settings.EnableVibrations)
            {
                VibrateController.Default.Start(TimeSpan.FromSeconds(.5));
            }

            if (Settings.IsAlarmOn)
            {
                TimeSpan timeToAlarm = Settings.AlarmTime.Value.TimeOfDay - current.TimeOfDay;

                // let the alarm sound up to 60 seconds after the designated time (in case the app
                // wasn't running at the beginning of the minute, or it was on a different page)
                if (!_tappedAlarmOff && _alarmSound.State != SoundState.Playing && timeToAlarm.TotalSeconds <= 0 &&
                    timeToAlarm.TotalSeconds > -60)
                {
                    _alarmSound.Play();
                    return; // don't bother with snooze
                }
            }

            if (Settings.SnoozeTime.IsSet)
            {
                // ReSharper disable PossibleInvalidOperationException
                // NOTE: Settings.SnoozeTime.IsSet => Settings.SnoozeTime.Value != null
                TimeSpan timeToSnooze = Settings.SnoozeTime.Value.Value.TimeOfDay - current.TimeOfDay;
                // ReSharper restore PossibleInvalidOperationException

                // let the snoozed alarm go off up to 60 seconds after the designated time (in case the
                // app wasn't running at the beginning of the minute, or it was on a different page)
                if (_alarmSound.State != SoundState.Playing && timeToSnooze.TotalSeconds <= 0 &&
                    timeToSnooze.TotalSeconds > -60)
                {
                    _alarmSound.Play();
                }
            }
        }

        private bool IsMatchingOrientation(PageOrientation orientation)
        {
            return Orientation == orientation;
        }

        #region - ApplicationBar handlers -

        private void AlarmButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AlarmPage.xaml", UriKind.Relative));
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Handles the 'Lock Orientation' button click.
        /// </summary>
        /// <param name="sender">The object which triggered the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> objet that contains the event data.</param>
        private void OrientationLockButton_Click(object sender, EventArgs e)
        {
            var orientationLockButton = (ApplicationBarIconButton)sender;

            // check the value of SupportedOrientations to see if we're currently "locked" to
            // a value other than PortraitOrLandscape
            if (SupportedOrientations != SupportedPageOrientation.PortraitOrLandscape)
            {
                // we are locked, so unlock now
                SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;

                // change the state of the ApplicationBar button to reflect the new state
                orientationLockButton.Text = "lock screen";
                orientationLockButton.IconUri = new Uri("/Images/appbar.orientationUnlocked.png", UriKind.Relative);
            }
            else
            {
                // we are unlocked, so lock to the current orientation now
                SupportedOrientations = IsMatchingOrientation(PageOrientation.PortraitUp)
                                            ? SupportedPageOrientation.Portrait
                                            : SupportedPageOrientation.Landscape;

                // change the state of the ApplicationBar button to reflect the new state
                orientationLockButton.Text = "unlock";
                orientationLockButton.IconUri = new Uri("/Images/appbar.orientationLocked.png", UriKind.Relative);
            }

            // remember the new setting after the page has been left
            Settings.SupportedOrientations.TrySet(SupportedOrientations);
        }

        private void InsructionsMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
