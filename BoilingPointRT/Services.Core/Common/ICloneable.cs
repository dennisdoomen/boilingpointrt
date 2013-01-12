using System;

namespace BoilingPointRT.Services.Common
{
    /// <summary>
    /// Generic implementation of <see cref="ICloneable"/>
    /// </summary>
    public interface ICloneable<out T>
    {
        /// <summary>
        /// Creates a shallow or deep copy of the current object.
        /// </summary>
        T Clone();
    }
}