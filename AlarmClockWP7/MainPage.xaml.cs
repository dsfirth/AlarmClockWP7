using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AlarmClockWP7
{
    public partial class MainPage
    {
        // DispatcherTimer: update the display every second.
        private readonly DispatcherTimer _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };

        private readonly TextBlock[] _dayOfWeekTextBlocks = new TextBlock[7];
        private static readonly string[] DayOfWeekText = new[] { "SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT" };
        //private bool _tappedAlarmOff;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            _timer.Tick += Timer_Tick;
            _timer.Start();

            // Initialize the alarm sound effect
            SoundEffects.Initialize();

            // Add the 7 day-of-week text blocks here, assigning them to an array.
            for (int i = 0; i < _dayOfWeekTextBlocks.Length; i++)
            {
                _dayOfWeekTextBlocks[i] = new TextBlock { Text = DayOfWeekText[i], Style = DayOfWeekStyle };
                Grid.SetColumn(_dayOfWeekTextBlocks[i], i + 1);

                LayoutRoot.Children.Add(_dayOfWeekTextBlocks[i]);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // Restore the ability for the screen to auto-lock when on other pages
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Enabled;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

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

            RefreshDisplay();

            // Don't wait for the next tick.
            Timer_Tick(this, EventArgs.Empty);
        }

        protected override void OnOrientationChanged(OrientationChangedEventArgs e)
        {
            if ((e.Orientation & PageOrientation.Portrait) == PageOrientation.Portrait)
            {
            }
            base.OnOrientationChanged(e);
            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            if (IsMatchingOrientation(PageOrientation.PortraitUp))
            {
                // Adjust the margins for portrait.
                LeftMargin.Width = new GridLength(12);
                RightMargin.Width = new GridLength(12);
                // Set the font size accordingly.
                if (Settings.ShowSeconds)
                {
                    CurrentTimeDisplay.FontSize = 182;
                }
                else
                {
                    CurrentTimeDisplay.FontSize = 223;
                }
            }
            else
            {
                // Adjust the margins for landscape.
                LeftMargin.Width = new GridLength(92);
                RightMargin.Width = new GridLength(92);
                // Set the font size accordingly.
                if (Settings.ShowSeconds)
                {
                    CurrentTimeDisplay.FontSize = 251;
                }
                else
                {
                    CurrentTimeDisplay.FontSize = 307;
                }
            }

            AlarmTimeDisplay.FontSize = CurrentTimeDisplay.FontSize / 2;

            // Respect the settings n the two time displays.
            CurrentTimeDisplay.Show24Hours = Settings.Show24Hours;
            AlarmTimeDisplay.Show24Hours = Settings.Show24Hours;
            CurrentTimeDisplay.ShowSeconds = Settings.ShowSeconds;
            CurrentTimeDisplay.Initialize();
            AlarmTimeDisplay.Initialize();

            {
                // No alarm, no snooze.
                AlarmOnTextBlock.Opacity = .1;
                SnoozeUntilTextBlock.Opacity = .1;
                AlarmTimeDisplay.Time = null;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var current = DateTime.Now;

            // Refresh the current time.
            CurrentTimeDisplay.Time = current;

            // Keep the day of the week up-to-date.
            for (int i = 0; i < _dayOfWeekTextBlocks.Length; i++)
            {
                _dayOfWeekTextBlocks[i].Opacity = .2;
            }
            _dayOfWeekTextBlocks[(int) current.DayOfWeek].Opacity = 1;

            // If the alarm sound...
        }

        bool IsMatchingOrientation(PageOrientation orientation)
        {
            return Orientation == orientation;
        }

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
        /// <param name="sender"></param>
        /// <param name="e">An <see cref="EventArgs"/> objet that contains the event data.</param>
        private void OrientationLockButton_Click(object sender, EventArgs e)
        {
            var orientationLockButton = (ApplicationBarIconButton)sender;

            // Check the value of SupportedOrientations to see if we're currently "locked" to
            // a value other than PortraitOrLandscape.
            if (SupportedOrientations != SupportedPageOrientation.PortraitOrLandscape)
            {
                // We are locked, so unlock now.
                SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;

                // Change the state of the ApplicationBar button to reflect the new state.
                orientationLockButton.Text = "lock screen";
                orientationLockButton.IconUri = new Uri("/Images/appbar.orientationUnlocked.png", UriKind.Relative);
            }
            else
            {
                // We are unlocked, so lock to the current orientation now.
                if (IsMatchingOrientation(PageOrientation.PortraitUp))
                {
                    SupportedOrientations = SupportedPageOrientation.Portrait;
                }
                else
                {
                    SupportedOrientations = SupportedPageOrientation.Landscape;
                }

                // Change the state o the ApplicationBar button to reflect the new state.
                orientationLockButton.Text = "unlock";
                orientationLockButton.IconUri = new Uri("/Images/appbar.orientationLocked.png", UriKind.Relative);
            }

            //// Remember the new setting after the page has been left.
            //Settings.SupportedOrientations.Value = this.SupportedOrientations;
        }

        private void InsructionsMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}