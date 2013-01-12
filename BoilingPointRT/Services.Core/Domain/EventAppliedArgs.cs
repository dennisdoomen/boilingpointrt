using System;

namespace BoilingPointRT.Services.Domain
{
    /// <summary>
    /// Represents the <see cref="IDomainEvent"/> that was applied to an <see cref="IEventSource"/>
    /// when the <see cref="IEventSource.EventApplied"/> occurred.
    /// </summary>
    public class EventAppliedArgs : EventArgs
    {
        public EventAppliedArgs(IDomainEvent @event)
        {
            Event = @event;
        }

        public IDomainEvent Event { get; private set; }
    }
}