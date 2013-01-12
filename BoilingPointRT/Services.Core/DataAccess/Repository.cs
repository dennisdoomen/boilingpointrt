using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BoilingPointRT.Services.DataAccess;
using BoilingPointRT.Services.Domain;
using BoilingPointRT.Services.ExceptionHandling;

using eVision.VisionControl.Interfaces.Shell.Core.Domain;

namespace eVision.VisionControl.Interfaces.DataAccess
{
    public abstract class Repository<T> : IQueryable<T> where T : class, IPersistable
    {
        protected abstract IQueryable<T> Entities { get; }

        /// <summary>
        /// Returns the total number of entities in the repository.
        /// </summary>
        public int Count
        {
            get { return Entities.Count(); }
        }

        public Type ElementType
        {
            get { return Entities.ElementType; }
        }

        public Expression Expression
        {
            get { return Entities.Expression; }
        }

        public IQueryProvider Provider
        {
            get { return Entities.Provider; }
        }

        public void AddRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Add(entity);
            }
        }

        public void Add(T entity)
        {
            AddAggregateRoot(entity);
        }

        protected abstract void AddAggregateRoot(T entity);

        public void RemoveRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Remove(entity);
            }
        }

        public void Remove(T entity)
        {
            RemoveAggregateRoot(entity);
        }

        public void Clear()
        {
            RemoveRange(Entities);
        }

        protected abstract void RemoveAggregateRoot(T entity);

        public IEnumerator<T> GetEnumerator()
        {
            return Entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Entities.GetEnumerator();
        }
    }
}