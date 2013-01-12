using System;
using System.Collections.Generic;

using BoilingPointRT.Services.Common;

using eVision.VisionControl.Interfaces.Shell.Core.Domain;

namespace BoilingPointRT.Services.Domain
{
    /// <summary>
    /// Represents an aggregate root which changes are persisted as a collection of <see cref="IDomainEvent" />.
    /// </summary>
    /// <typeparam name="TState">
    /// The type of the object that will hold the state of the aggregate.
    /// </typeparam>
    public abstract class EventSource<TState> : IEventSource
        where TState : AggregateState, new()
    {
        #region Private Definitions

        private readonly List<Event> changes = new List<Event>();
        private long committedVersion;
        private Guid? id;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSource{TState}" /> class.
        /// </summary>
        protected EventSource()
        {
            State = new TState();
        }

        /// <summary>
        /// Occurs when a change is applied to the aggregate.
        /// </summary>
        public virtual event EventHandler<EventAppliedArgs> EventApplied = delegate { };

        /// <summary>
        /// Gets an identifier that identifies the underlying event stream.
        /// </summary>
        /// <remarks>
        /// The concrete implementation must have a property representing the functional key identified with
        /// <see cref="IdentityAttribute"/>. If that property is not of type <see cref="Guid"/>, it will convert it using
        /// hash algorithm.
        /// </remarks>
        public virtual Guid StreamId
        {
            get
            {
                if (!id.HasValue)
                {
                    object key = AggregateRootReflector.GetKey(this);
                    id = key.ToDeterministicGuid(GetType());
                }

                return id.Value;
            }
        }

        /// <summary>
        /// Represents an object that contains the state of the aggregate and processes the events.
        /// </summary>
        protected TState State { get; private set; }

        /// <summary>
        /// Applies the specified event to the aggregate.
        /// </summary>
        public virtual void Apply<TEvent>(TEvent @event) where TEvent : Event
        {
            changes.Add(@event);

            @event.Version = Version;

            State.Process(@event);

            DomainEvents.Raise(@event);

            EventApplied(this, new EventAppliedArgs(@event));
        }

        /// <summary>
        /// Loads the aggregate with a collection of events in the provided order to represent the state of the 
        /// aggregate when it was last committed.
        /// </summary>
        public virtual void Load(long committedVersion, IEnumerable<IDomainEvent> events)
        {
            changes.Clear();

            foreach (var @event in events)
            {
                State.Process(@event);
            }

            this.committedVersion = committedVersion;
        }

        /// <summary>
        /// Gets the changes applied to the aggregate since it was initially loaded or the last time committed.
        /// </summary>
        public virtual IEnumerable<Event> GetChanges()
        {
            return changes;
        }

        /// <summary>
        /// Marks the aggregate as committed.
        /// </summary>
        public virtual void MarkAsCommitted(long committedVersion)
        {
            changes.Clear();
            this.committedVersion = committedVersion;
        }

        /// <summary>
        /// Gets the version of the aggregate at the time it was loaded from the underlying data store
        /// </summary>
        public virtual long CommittedVersion
        {
            get { return committedVersion; }
        }

        /// <summary>
        /// Gets the version of the entire aggregate, including any uncommitted changes.
        /// </summary>
        public virtual long Version
        {
            get { return committedVersion + changes.Count; }
            set
            {
                // NOTE: temporary setter that is necessary for event sourced aggrgeates that use the [UseNHibernate] attribute
                committedVersion = value;
            }
        }
    }
}