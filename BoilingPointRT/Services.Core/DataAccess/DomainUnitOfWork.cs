using System;
using System.Collections.Generic;
using System.Linq;

using BoilingPointRT.Services.Domain;

namespace BoilingPointRT.Services.DataAccess
{
    /// <summary>
    /// Represents a unit of work for keeping track of changes made to the domain model of a system.
    /// </summary>
    public class DomainUnitOfWork : UnitOfWork
    {
        private readonly IDomainMapper mapper;

        #region Private Definition

        private readonly Dictionary<Type, HashSet<object>> versionAssertedEntities = new Dictionary<Type, HashSet<object>>();

        #endregion

        protected DomainUnitOfWork()
        {
        }

        public DomainUnitOfWork(IDomainMapper mapper)
        {
            this.mapper = mapper;
        }

        public object Get(Type entityType, object key)
        {
            return mapper.Get(entityType, key);
        }

        public T Get<T>(object key, long version)
        {
            // To facilitate command batching, the version of an aggregate only needs to be checked once within a unit of work.
            if (HasEntityVersionPreviouslyBeenAsserted(typeof(T), key))
            {
                return (T)mapper.Get(typeof(T), key);
            }

            MarkAsVersionAsserted(typeof(T), key);

            return (T)mapper.Get(typeof(T), key, version);
        }

        private bool HasEntityVersionPreviouslyBeenAsserted(Type entityType, object key)
        {
            var query =
                from type in versionAssertedEntities.Keys
                where type.IsAssignableFrom(entityType) || entityType.IsAssignableFrom(type)
                select versionAssertedEntities[type].Contains(key);

            return query.FirstOrDefault();
        }

        private void MarkAsVersionAsserted(Type entityType, object key)
        {
            if (!versionAssertedEntities.ContainsKey(entityType))
            {
                versionAssertedEntities.Add(entityType, new HashSet<object>());
            }

            versionAssertedEntities[entityType].Add(key);
        }

        public bool Exists<T>(object key)
        {
            return mapper.Exists(typeof(T), key);
        }

        public T Get<T>(object key)
        {
            return (T)mapper.Get(typeof(T), key);
        }

        /// <summary>
        /// Adds the specified aggregate root to the unit of work.
        /// </summary>
        /// <remarks>
        /// If the aggregate is already part of the unit of work, then the call is ignored.
        /// </remarks>
        public void Add(IEventSource aggregate)
        {
            mapper.Add(aggregate);
        }
    }
}