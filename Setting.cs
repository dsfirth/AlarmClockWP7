using System;

namespace AlarmClockWP7
{
    using System.IO.IsolatedStorage;

    /// <summary>
    /// Encapsulates a key/value pair stored in Isolated Storage ApplicationSettings
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    public class Setting<T>
    {
        private readonly string _key;
        private readonly T _defaultValue;
        private T _value;
        private bool _hasValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="Setting{T}"/> class.
        /// </summary>
        /// <param name="key">The key of the setting item; used for saving the setting to <see cref="IsolatedStorageSettings.ApplicationSettings"/>.</param>
        /// <param name="defaultValue">The default value for this setting.</param>
        public Setting(string key, T defaultValue)
        {
            _key = key;
            _defaultValue = defaultValue;
        }

        /// <summary>
        /// Causes the current instance to "discard" the in-memory value, forcing the next call to
        /// the <see cref="Value"/> getter to re-fetch from <see cref="IsolatedStorageSettings.ApplicationSettings"/>.
        /// </summary>
        public void ForceRefresh()
        {
            _hasValue = false;
        }

        public void Reset()
        {
            Value = DefaultValue;
        }

        public string Key
        {
            get { return _key; }
        }

        public T Value
        {
            get
            {
                // Check for the cached value
                if (!_hasValue)
                {
                    // Try to get the value from Isolated Storage
                    if (!IsolatedStorageSettings.ApplicationSettings.TryGetValue(_key, out _value))
                    {
                        // It hasn’t been set yet
                        _value = _defaultValue;
                        IsolatedStorageSettings.ApplicationSettings[_key] = _value;
                    }
                    _hasValue = true;
                }
                return _value;
            }
            set
            {
                // Save the value to Isolated Storage
                IsolatedStorageSettings.ApplicationSettings[_key] = value;
                _value = value;
                _hasValue = true;
            }
        }

        public T DefaultValue
        {
            get { return _defaultValue; }
        }
    }

    public static class SettingExtensions
    {
        public static bool TrySet<T>(this Setting<T> setting, T? value) where T : struct
        {
            if (value.HasValue)
            {
                setting.Value = value.Value;
                return true;
            }

            return false;
        }
    }
}
