using System;

namespace BoilingPointRT.Services.Domain
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}