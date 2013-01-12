using System;
using System.Collections.Generic;

namespace BoilingPointRT.Services.Domain
{
    /// <summary>
    /// Represents an aggregate root which changes are persisted as a collection of <see cref="IDomainEvent" />.
    /// </summary>
    public interface IEventSource : IAggregateRoot
    {
        /// <summary>
        /// Occurs when a change is applied to the aggregate.
        /// </summary>
        event EventHandler<EventAppliedArgs> EventApplied;

        /// <summary>
        /// Gets the version of the aggregate at the time it was loaded from the underlying data store
        /// </summary>
        long CommittedVersion { get; }

        /// <summary>
        /// Gets an identifier that identifies the underlying event stream.
        /// </summary>
        /// <remarks>
        /// The concrete implementation must have a property representing the functional key identified with
        /// <see cref="IdentityAttribute"/>. If that property is not of type <see cref="Guid"/>, it will convert it using
        /// hash algorithm.
        /// </remarks>
        Guid StreamId { get; }

        /// <summary>
        /// Gets the changes applied to the aggregate since it was initially loaded or the last time committed.
        /// </summary>
        IEnumerable<Event> GetChanges();

        /// <summary>
        /// Marks the aggregate as committed.
        /// </summary>
        void MarkAsCommitted(long committedVersion);

        /// <summary>
        /// Loads the aggregate with a collection of events in the provided order to represent the state of the 
        /// aggregate when it was last committed.
        /// </summary>
        void Load(long committedVersion, IEnumerable<IDomainEvent> events);
    }
}