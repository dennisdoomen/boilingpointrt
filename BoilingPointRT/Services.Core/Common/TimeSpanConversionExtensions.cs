using System;

namespace BoilingPointRT.Services.Common
{
    public static class TimeSpanConversionExtensions
    {
        /// <summary>
        /// Converts the current <see cref="TimeSpan"/> value to its equivalent string representation for the total elapsed
        /// hours and minutes in the format "H:mm".
        /// </summary>
        /// <example>
        /// new TimeSpan(8, 59, 0).ToElapsedHoursString() == "8:59";<br/>
        /// new TimeSpan(1, 6, 45, 0).ToElapsedHoursString() == "30:45";
        /// </example>
        /// <returns>The string representation of the elapsed hourse and minutes of this instance.</returns>
        public static string ToElapsedTimeString(this TimeSpan source)
        {
            int hours = (int)Math.Floor(source.TotalMinutes / 60);
            int minutes = (int)Math.Floor(source.TotalMinutes % 60);

            return string.Format("{0}:{1:00}", hours, minutes);
        }

        /// <summary>
        /// Converts the current <see cref="string"/> representation of the total elapsed hours and minutes into a
        /// <see cref="TimeSpan"/>. The string must be in the format "H:mm"
        /// </summary>
        /// <example>
        /// "8:59".ConvertToTimeSpan() == new TimeSpan(8, 59, 0);<br/>
        /// "30:45".ConvertToTimeSpan() == new TimeSpan(1, 6, 45, 0);
        /// </example>
        /// <returns></returns>
        public static TimeSpan ConvertToTimeSpan(this string source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            int seperatorIndex = source.IndexOf(":");
            if (seperatorIndex < 0)
            {
                throw new ArgumentException(
                    string.Format("Cannot convert string '{0}' into a TimeSpan, because the ':' seperator is missing.)", source));
            }

            return ConvertToTimeSpan(source, seperatorIndex);
        }

        private static TimeSpan ConvertToTimeSpan(string source, int seperatorIndex)
        {
            Exception error;
            try
            {
                string hoursPart = source.Substring(0, seperatorIndex);
                string minutesPart = source.Substring(seperatorIndex + 1);

                int hours = int.Parse(hoursPart);
                int minutes = int.Parse(minutesPart);

                return new TimeSpan(hours, minutes, 0);
            }
            catch (ArgumentException ex)
            {
                error = ex;
            }
            catch (FormatException ex)
            {
                error = ex;
            }
            catch (ArithmeticException ex)
            {
                error = ex;
            }

            throw new ArgumentException(string.Format("Cannot convert string '{0}' into a TimeSpan.", source), error);
        }
    }
}