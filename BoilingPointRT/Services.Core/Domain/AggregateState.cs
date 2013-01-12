using System;
using System.Reflection;

using BoilingPointRT.Services.Common;

namespace BoilingPointRT.Services.Domain
{
    /// <summary>
    /// Represents the state of an aggregate based on <see cref="IEventSource"/> and processes both current and historical
    /// events.
    /// </summary>
    public abstract class AggregateState
    {
        /// <summary>
        /// Applies the specified <see cref="IDomainEvent"/> to the current state by invoking the appropriate When method
        /// that takes a particular event.
        /// </summary>
        public void Process(IDomainEvent @event)
        {
            if (!@event.GetType().HasAttribute<ObsoleteAttribute>())
            {
                MethodInfo info = EventMappingCache.GetHandlerFor(GetType(), @event.GetType());

                try
                {
                    info.Invoke(this, new[] { @event });
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.Unwrap();
                }
            }
        }
    }
}