namespace BoilingPointRT.Services.Domain
{
    /// <summary>
    /// Handles the domain event <typeparamref name="T"/>.
    /// </summary>
    public interface Handles<T> where T : IDomainEvent
    {
        void Handle(T @event);
    }
}