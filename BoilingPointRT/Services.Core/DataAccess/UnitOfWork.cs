using System;
using System.Threading;

namespace BoilingPointRT.Services.DataAccess
{
    public abstract class UnitOfWork : IDisposable
    {
        protected IDataMapper mapper;
        private readonly object syncObject = new object();
        private int referenceCount;
        private readonly long id;
        private static long nextId = 1;

        /// <summary>
        /// Occurs when the unit of work is completed disposed.
        /// </summary>
        public EventHandler Disposing = delegate
        {
        };

        protected UnitOfWork()
        {
            id = Interlocked.Increment(ref nextId);
            referenceCount = 1;
        }

        protected UnitOfWork(IDataMapper mapper)
            : this()
        {
            this.mapper = mapper;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this unit of work has already been disposed.
        /// </summary>
        internal bool IsDisposed { get; private set; }

        public IDataMapper Mapper
        {
            get { return mapper; }
        }

        /// <summary>
        /// Connects an existing unit of work to a <see cref="IDataMapper"/> which lifecycle control is out of scope.
        /// </summary>
        public void ConnectToSharedMapper(IDataMapper mapper)
        {
            this.mapper = mapper;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Removes an individual entity from the current unit of work.
        /// </summary>
        public void Evict(IPersistable aggregate)
        {
            mapper.Evict(aggregate);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            lock (syncObject)
            {
                referenceCount--;
                if (referenceCount == 0)
                {
                    Disposing(this, EventArgs.Empty);
                    mapper.Dispose();
                    IsDisposed = true;

                    GC.SuppressFinalize(this);
                }
            }
        }

        internal void IncreaseReferenceCount()
        {
            lock (syncObject)
            {
                referenceCount++;
            }
        }

        public void SubmitChanges()
        {
            mapper.SubmitChanges();
        }
    }
}