using System;
using System.Windows;

namespace AlarmClockWP7
{
    public partial class TimeDisplay
    {
        private DateTime? _time;

        public TimeDisplay()
        {
            InitializeComponent();
            Time = null;
        }

        public void Initialize()
        {
            if (!ShowSeconds)
            {
                // Remove the seconds display
                SecondsRun.Text = null;
                SecondsBackgroundRun.Text = null;
            }

            // Hide AM and PM in 24-hour mode
            AMTextBlock.Visibility = Show24Hours ? Visibility.Collapsed : Visibility.Visible;
            PMTextBlock.Visibility = Show24Hours ? Visibility.Collapsed : Visibility.Visible;

            // The seconds font size is always half of whatever the main font size is
            SecondsBackgroundRun.FontSize = SecondsRun.FontSize = FontSize/2;
        }

        public bool Show24Hours { get; set; }

        public bool ShowSeconds { get; set; }

        public DateTime? Time
        {
            get { return _time; }
            set
            {
                _time = value;

                if (_time == null)
                {
                    // Clear everything
                    TimeRun.Text = null;
                    SecondsRun.Text = null;
                    AMTextBlock.Opacity = .1;
                    PMTextBlock.Opacity = .1;
                    return;
                }

                string formatString = Show24Hours ? "H:mm" : "h:mm";
                DateTime time = _time.Value;

                // The hour needs a leading space if it ends up being only one digit
                if ((Show24Hours && time.Hour < 10) || (!Show24Hours && time.Hour%12 < 10 && time.Hour%12 > 0))
                {
                    formatString = " " + formatString;
                }

                TimeRun.Text = time.ToString(formatString);

                if (ShowSeconds)
                {
                    SecondsRun.Text = time.ToString("ss");
                }

                if (!Show24Hours)
                {
                    // Show either AM or PM
                    if (time.Hour < 12)
                    {
                        AMTextBlock.Opacity = 1;
                        PMTextBlock.Opacity = .1;
                    }
                    else
                    {
                        AMTextBlock.Opacity = .1;
                        PMTextBlock.Opacity = 1;
                    }
                }
            }
        }
    }
}
