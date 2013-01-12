using BoilingPointRT.Services.DataAccess;

namespace BoilingPointRT.Services.Domain
{
    public interface IVersionedEntity : IPersistable
    {
        long Version { get; }
    }
}