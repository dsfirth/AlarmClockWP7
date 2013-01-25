using AlarmClockWP7.ViewModels;

namespace AlarmClockWP7.Views
{
    /// <summary>
    /// Code-behind class for /Views/SettingsPage.xaml; page for selecting the AlarmClockWP7's application settings.
    /// </summary>
    public partial class SettingsPage
    {
        // ctor
        public SettingsPage()
        {
            InitializeComponent();
            DataContext = new SettingsViewModel();
        }
    }
}
