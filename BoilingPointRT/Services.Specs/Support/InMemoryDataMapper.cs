using System;
using System.Collections.Generic;
using System.Linq;

using BoilingPointRT.Services.Common;
using BoilingPointRT.Services.DataAccess;
using BoilingPointRT.Services.Domain;
using BoilingPointRT.Services.ExceptionHandling;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using eVision.VisionControl.Interfaces.DataAccess;

namespace BoilingPointRT.Services.Core.Specs.Support
{
    public class InMemoryDataMapper : IDomainMapper
    {
        #region Private Definitions

        private readonly List<IPersistable> committed = new List<IPersistable>();
        private readonly List<IPersistable> uncommittedInserts = new List<IPersistable>();
        private readonly List<IPersistable> uncommittedDeletes = new List<IPersistable>();

        [ThreadStatic]
        public static HashSet<IPersistable> ambientEntities;

        #endregion

        public InMemoryDataMapper()
        {
            foreach (var ambientEntity in AmbientEntities)
            {
                AddCommitted(ambientEntity);
            }
        }

        public InMemoryDataMapper(params IPersistable[] commitedEntities)
            : this()
        {
            foreach (var commitedEntity in commitedEntities)
            {
                AddCommitted(commitedEntity);
            }
        }

        public static HashSet<IPersistable> AmbientEntities
        {
            get
            {
                if (ambientEntities == null)
                {
                    ambientEntities = new HashSet<IPersistable>();
                }

                return ambientEntities;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this data mapper is currently maintaining a transaction.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has transaction; otherwise, <c>false</c>.
        /// </value>
        public bool HasTransaction { get; set; }

        /// <summary>
        /// Gets a value indicating whether the data mapper still has changes to commit.
        /// </summary>
        public bool HasChanges
        {
            get { return uncommittedInserts.Any() || uncommittedDeletes.Any(); }
        }

        public bool Saved { get; private set; }
        public bool IsDisposed { get; private set; }

        public IEnumerable<T> GetCommittedEntities<T>() where T : class
        {
            return committed.OfType<T>();
        }

        public IEnumerable<T> GetUncommittedEntities<T>() where T : class
        {
            return uncommittedInserts.OfType<T>();
        }

        public IEnumerable<T> GetUncommittedDeletes<T>() where T : class
        {
            return uncommittedDeletes.OfType<T>();
        }

        public void AddCommitted(IPersistable entity)
        {
            if (!committed.Contains(entity))
            {
                committed.Add(entity);
            }
        }

        public void Add(IEventSource aggregate)
        {
            if (!uncommittedInserts.Contains(aggregate))
            {
                uncommittedInserts.Add(aggregate);
            }
        }

        public object Get(Type aggregateRootType, object key, long version = VersionedEntity.IgnoredVersion)
        {
            object entity = committed.SingleOrDefault(c => aggregateRootType.IsInstanceOfType(c) && GetKeyValue(c).Equals(key));
            if (entity == null)
            {
                throw new ApplicationErrorException(Error.RecordDoesNotExistAnymore)
                {
                    { "EntityType", aggregateRootType.Name },
                    { "Key", key },
                    { "Version", version }
                };
            }

            if (!version.Equals(VersionedEntity.IgnoredVersion))
            {
                var versionedEntity = entity as IVersionedEntity;
                if (versionedEntity == null)
                {
                    throw new InvalidOperationException(aggregateRootType.Name + " is not a versioned entity");
                }

                if (versionedEntity.Version != version)
                {
                    throw new ApplicationErrorException(Error.RecordIsChangedByAnotherUser);
                }
            }

            return entity;
        }

        public bool Exists(Type aggregateRootType, object key)
        {
            return committed.Any(c => aggregateRootType.IsInstanceOfType(c) && GetKeyValue(c).Equals(key));
        }

        public void Evict(IEventSource aggregate)
        {
            
        }

        public object GetWithLock(Type aggregateRootType, object key, long version)
        {
            return Get(aggregateRootType, key, version);
        }

        private static object GetKeyValue(object o)
        {
            string keyPropertyName = AggregateRootReflector.GetKeyPropertyName(o.GetType());

            return o.GetType().GetProperty(keyPropertyName).GetValue(o, null);
        }

        /// <summary>
        /// If the data mapper <see cref="IDataMapper.HasChanges"/>, then it will persist all changes to the underlying data store.
        /// </summary>
        public void SubmitChanges()
        {
            committed.AddRange(uncommittedInserts);
            uncommittedInserts.Clear();
            committed.RemoveAll(e => uncommittedDeletes.Contains(e));
            uncommittedDeletes.Clear();
            Saved = true;
        }

        /// <summary>
        /// Removes all entities from the current unit of work.
        /// </summary>
        public void EvictAll()
        {
            committed.Clear();
            uncommittedDeletes.Clear();
            uncommittedInserts.Clear();
        }

        /// <summary>
        /// Removes an individual entity from the current unit of work.
        /// </summary>
        public void Evict(IPersistable aggregate)
        {
            committed.Remove(aggregate);
            uncommittedDeletes.Remove(aggregate);
            uncommittedInserts.Remove(aggregate);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            IsDisposed = true;
        }

        private sealed class InMemoryRepository<T> : Repository<T> where T : class, IPersistable
        {
            private readonly InMemoryDataMapper mapper;

            public InMemoryRepository(InMemoryDataMapper mapper)
            {
                this.mapper = mapper;
            }

            protected override IQueryable<T> Entities
            {
                get { return mapper.committed.OfType<T>().AsQueryable(); }
            }

            protected override void AddAggregateRoot(T entity)
            {
                if (mapper.committed.Contains(entity))
                {
                    Assert.Fail("Entity already exist.");
                }

                mapper.uncommittedInserts.Add(entity);
            }

            protected override void RemoveAggregateRoot(T entity)
            {
                if (!mapper.committed.Contains(entity))
                {
                    Assert.Fail("Entity does not exist.");
                }

                mapper.uncommittedDeletes.Add(entity);
            }
        }

        public static void AddAmbientEntities(params IPersistable[] persistables)
        {
            foreach (IPersistable persistable in persistables)
            {
                AmbientEntities.Add(persistable);
            }
        }
    }
}
