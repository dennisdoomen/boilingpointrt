using System.Collections.Generic;

namespace BoilingPointRT.Services.Domain
{
    /// <summary>
    /// Generic interface for enumerating all items of a <see cref="Descriptor"/>
    /// </summary>
    public interface IDescriptorEnumerator<out TDescriptor> where TDescriptor : Descriptor
    {
        /// <summary>
        /// Gets all possible descriptors values for <typeparamref name="TDescriptor"/>.
        /// </summary>
        IEnumerable<TDescriptor> GetAll();
    }
}