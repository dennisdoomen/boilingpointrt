using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BoilingPointRT.Services.Common
{
    public static class StringExtensions
    {
        /// <summary>
        /// Indicates if the current <see cref = "string" /> is NOT <c>null</c> or <see cref = "string.Empty" />.
        /// </summary>
        public static bool IsNotNullOrEmpty(this string source)
        {
            return !string.IsNullOrEmpty(source);
        }

        /// <summary>
        /// Indicates if the current <see cref = "string" /> is <c>null</c> or <see cref = "string.Empty" />.
        /// </summary>
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        /// <summary>
        ///   Indicates if all characters in the supplied <paramref name = "value" /> are UPPER case.
        /// </summary>
        public static bool IsUpperCase(this IEnumerable<char> value)
        {
            return !value.Any(char.IsLower);
        }

        /// <summary>
        ///   Indicates if all characters in the supplied <paramref name = "value" /> are lower case.
        /// </summary>
        public static bool IsLowerCase(this IEnumerable<char> value)
        {
            return !value.Any(char.IsUpper);
        }

        /// <summary>
        /// Joins an arbitrary collection of strings using the specified separator.
        /// </summary>
        public static string Join(this IEnumerable<string> values, string separator)
        {
            return string.Join(separator, values.ToArray());
        }

        /// <summary>
        /// Gets a string from the resource provider which is split on newlines.
        /// </summary>
        public static string[] SplitOnNewLine(this string multiLineString)
        {
            return multiLineString.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Removes any white spaces from the current string.
        /// </summary>
        public static string RemoveSpaces(this string source)
        {
            return source.Replace(" ", "");
        }

        /// <summary>
        /// Normalizes a string between <c>null</c> and <see cref="string.Empty"/>.
        /// </summary>
        /// <param name="text">The source string.</param>
        /// <returns><c>null</c> when the specified string is empty; the original string otherwise.</returns>
        public static string NullWhenEmpty(this string text)
        {
            return text == string.Empty ? null : text;
        }

        /// <summary>
        /// Replaces each named format item in a specified string with a value from a dictionary. If the specified string 
        /// contains indexed items, for example: {0}, it will use the normal string.format functionality.
        /// </summary>
        /// <param name="formatString">The format string containing the named or indexed format items.</param>
        /// <param name="namedValues">A dictionary containing the named values.</param>
        /// <returns>A formatted string.</returns>
        public static string FormatWith(this string formatString, Dictionary<string, string> namedValues)
        {
            const string indexedFormatStringRegEx = @"(?<start>\{)+(?<property>\d+)(?<end>\})+";
            const string namedFormatStringRegEx = @"(?<start>\{)+(?<property>[\w\.\[\]]+)(?<end>\})+";

            if(Regex.IsMatch(formatString, indexedFormatStringRegEx))
            {
                return string.Format(formatString, namedValues.Values.ToArray());
            }

            return Regex.Replace(formatString, namedFormatStringRegEx, m => ResolveNamedValue(namedValues, m.Groups["property"].Value),
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        }

        private static string ResolveNamedValue(Dictionary<string, string> values, string key)
        {
            if (!values.ContainsKey(key))
            {
                throw new FormatException(string.Format("Dictionary does not contain a value with key: {0}", key));
            }

            object propertyValue = values[key];

            return (propertyValue != null) ? propertyValue.ToString() : string.Empty;
        }
    }
}