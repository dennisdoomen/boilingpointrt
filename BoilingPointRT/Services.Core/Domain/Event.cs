namespace BoilingPointRT.Services.Domain
{
    /// <summary>
    /// Convenient class for creating domain events that contain the version of an object.
    /// </summary>
    public abstract class Event : IDomainEvent
    {
        /// <summary>
        /// Gets or sets the version of the aggregate that this event applies to.
        /// </summary>
        public long Version { get; set; }
    }
}