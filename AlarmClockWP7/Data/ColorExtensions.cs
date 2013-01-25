using System.Windows.Media;

namespace AlarmClockWP7.Data
{
    /// <summary>
    /// Contains extension-methods related to Color.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Converts a hex value to its <see cref="Color"/> equivalent. See Remarks.
        /// </summary>
        /// <param name="argb">The hex value.</param>
        /// <returns></returns>
        /// <remarks>
        /// Calls to this method should look like 0xFF11FF11.ToColor().
        /// </remarks>
        public static Color ToColor(this uint argb)
        {
            return Color.FromArgb((byte)((argb & 0xff000000) >> 0x18),
                                  (byte)((argb & 0xff0000) >> 0x10),
                                  (byte)((argb & 0xff00) >> 8),
                                  (byte)(argb & 0xff));
        }

        /// <summary>
        /// Converts a hex value to its <see cref="SolidColorBrush"/> equivalent. See Remarks.
        /// </summary>
        /// <param name="argb">The hex value.</param>
        /// <returns></returns>
        /// <remarks>
        /// Calls to this method should look like 0xFF11FF11.ToSolidColorBrush().
        /// </remarks>
        public static SolidColorBrush ToSolidColorBrush(this uint argb)
        {
            return new SolidColorBrush(argb.ToColor());
        }
    }
}
