using System;
using System.Data;

using BoilingPointRT.Services.Domain;

namespace BoilingPointRT.Services.DataAccess
{
    public interface IDataMapper : IDisposable
    {
        /// <summary>
        /// Removes an individual entity from the current unit of work.
        /// </summary>
        void Evict(IPersistable persistable);

        /// <summary>
        /// If the domain mapper has changes, then it will persist all changes to the underlying data store.
        /// </summary>
        void SubmitChanges();
    }

    /// <summary>
    /// Provides an abstraction of an object-oriented data mapper such as required by the <see cref="DomainUnitOfWork" />
    /// class.
    /// </summary>
    public interface IDomainMapper : IDataMapper
    {
        /// <summary>
        /// Should add the specified aggregate root to the unit of work and keep track of its changes.
        /// </summary>
        /// <remarks>
        /// If the aggregate is already part of the unit of work, then the call should be ignored.
        /// </remarks>
        void Add(IEventSource aggregateRoot);

        /// <summary>
        /// Gets an instance of an entity based on its functional key and the version.
        /// </summary>
        object Get(Type aggregateRootType, object key, long version = VersionedEntity.IgnoredVersion);

        /// <summary>
        /// Check if a specific entity exists.
        /// </summary>
        bool Exists(Type aggregateRootType, object key);

        /// <summary>
        /// Removes all entities from the current unit of work.
        /// </summary>
        void EvictAll();
    }
}