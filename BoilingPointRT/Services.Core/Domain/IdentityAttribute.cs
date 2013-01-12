using System;

namespace BoilingPointRT.Services.Domain
{
    /// <summary>
    /// Marks a property as the aggregate root identity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IdentityAttribute : Attribute
    {
    }
}