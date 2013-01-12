using System;

namespace BoilingPointRT.Services.Common
{
    public static class GuidExtensions
    {
        /// <summary>
        ///   Indicates if the current <see cref = "Guid" /> is not <c>null</c> and not <see cref = "Guid.Empty" />.
        /// </summary>
        public static bool IsNotNullOrEmpty(this Guid? source)
        {
            return (source.HasValue && (source.Value != Guid.Empty));
        }

        /// <summary>
        /// Tries to parse the string and returns the created Guid, returns null if it cannot be parsed.
        /// </summary>
        public static Guid? ToNullableGuid(this string input)
        {
            Guid guid;
            Guid? nullableGuid = null;

            if (Guid.TryParse(input, out guid))
            {
                nullableGuid = guid;
            }

            return nullableGuid;
        }
    }
}