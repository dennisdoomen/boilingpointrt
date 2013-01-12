using System;
using System.Linq;

namespace BoilingPointRT.Services.Domain
{
    public static class DescriptorEnumeratorExtensions
    {
        /// <summary>
        /// Attempts to find the correct <see cref="TDescriptor"/> for a particular code.
        /// </summary>
        /// <returns>
        /// An instance of <see cref="TDescriptor"/> for the specified code. Throws if the code does not match any known descriptor.
        /// </returns>
        public static TDescriptor FromCode<TDescriptor>(this IDescriptorEnumerator<TDescriptor> enumerator, string code)
            where TDescriptor : Descriptor
        {
            TDescriptor descriptor = enumerator.GetAll().SingleOrDefault(d => d.Code.Equals(code));
            if (descriptor == null)
            {
                throw new ArgumentOutOfRangeException("code", string.Format("The code \"{0}\" does not match a known {1}.",
                    code, typeof(TDescriptor).Name));
            }

            return descriptor;
        }
    }
}