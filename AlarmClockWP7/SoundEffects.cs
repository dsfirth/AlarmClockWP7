namespace AlarmClockWP7
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Resources;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;

    public static class SoundEffects
    {
        public static void Initialize()
        {
            StreamResourceInfo alarmResInfo = Application.GetResourceStream(new Uri("Audio/alarm.wav", UriKind.Relative));
            Alarm = SoundEffect.FromStream(alarmResInfo.Stream);

            // required for XNA-based sound effects to work
            CompositionTarget.Rendering += (sender, args) => FrameworkDispatcher.Update();

            // also call once at the beginning
            FrameworkDispatcher.Update();
        }

        public static SoundEffect Alarm { get; private set; }
    }
}
