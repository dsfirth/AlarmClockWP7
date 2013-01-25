using System;
using System.ComponentModel;
using AlarmClockWP7.Annotations;
using AlarmClockWP7.Shared.Settings;

namespace AlarmClockWP7.ViewModels
{
    public class AlarmViewModel : INotifyPropertyChanged
    {
        public bool IsAlarmOn
        {
            get { return UserSettings.IsAlarmOn; }
            set
            {
                UserSettings.IsAlarmOn.TrySet(value);
                OnPropertyChanged("IsAlarmOn");
            }
        }

        public DateTime AlarmTime
        {
            get { return UserSettings.AlarmTime; }
            set
            {
                UserSettings.AlarmTime.TrySet(value);
                OnPropertyChanged("AlarmTime");
            }
        }

        #region - INotifyPropertyChanged implementation 0

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
