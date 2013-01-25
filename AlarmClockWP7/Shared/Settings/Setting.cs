using System;
using System.IO.IsolatedStorage;

namespace AlarmClockWP7.Shared.Settings
{
    /// <summary>
    /// Encapsulates a key/value pair stored in IsolatedStorage ApplicationSettings
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    public class Setting<T>
    {
        /// <summary>
        /// Defines the implicit <see cref="Setting{T}"/> to <typeparamref name="T"/> conversion operator.
        /// </summary>
        /// <param name="setting">A <see cref="Setting{T}"/> object to be converted.</param>
        /// <returns>The value contained in the <see cref="Setting{T}"/> object.</returns>
        public static implicit operator T(Setting<T> setting)
        {
            return setting.Value;
        }

        private readonly string _key;
        private readonly T _defaultValue;
        private T _value;
        private bool _hasValue; // indicates if the current instance has cached its value (in _value)

        /// <summary>
        /// Initializes a new instance of the <see cref="Setting{T}"/> class.
        /// </summary>
        /// <param name="key">The key of the setting item; used for saving the setting to <see cref="IsolatedStorageSettings.ApplicationSettings"/>.</param>
        public Setting(string key)
        {
            _key = key;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Setting{T}"/> class.
        /// </summary>
        /// <param name="key">The key of the setting item; used for saving the setting to <see cref="IsolatedStorageSettings.ApplicationSettings"/>.</param>
        /// <param name="defaultValue">The default value for this setting.</param>
        public Setting(string key, T defaultValue)
            : this(key)
        {
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

        /// <summary>
        /// Removes explicit setting value (thereby deferring to <see cref="DefaultValue"/> for its current value).
        /// </summary>
        public void Reset()
        {
            // remove the saved value from IsolatedStorage
            IsolatedStorageSettings.ApplicationSettings.Remove(_key);
            _hasValue = false;
        }

        public string Key
        {
            get { return _key; }
        }

        public T Value
        {
            get
            {
                // check for the cached value
                if (!_hasValue)
                {
                    // rry to get the value from IsolatedStorage
                    if (!IsolatedStorageSettings.ApplicationSettings.TryGetValue(_key, out _value))
                    {
                        // it hasn’t been set yet (don't save the DefaultValue to IsolatedStorage)
                        _value = DefaultValue;
                    }
                    _hasValue = true;
                }
                return _value;
            }
            set
            {
                // save the value to IsolatedStorage
                IsolatedStorageSettings.ApplicationSettings[_key] = value;
                _value = value;
                _hasValue = true;
            }
        }

        public T DefaultValue
        {
            get { return _defaultValue; }
        }

        public bool IsSet
        {
            get { return !Value.Equals(DefaultValue); }
        }
    }

    /// <summary>
    /// Provides a set of static methods for operating on a <see cref="Setting{T}"/> object.
    /// </summary>
    public static class SettingExtensions
    {
        /// <summary>
        /// Assigns the specified value to a <see cref="Setting{T}"/> object. A return value
        /// indicates whether the assignment was successful.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="setting">A <see cref="Setting{T}"/> to assign the value.</param>
        /// <param name="value">A nullable value to assign to the <see cref="Setting{T}"/> object.</param>
        /// <returns><c>true</c> if <paramref name="value"/> has a value and is successfully assigned to the <see cref="Setting{T}"/> object; otherwise, <c>false</c>.</returns>
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
