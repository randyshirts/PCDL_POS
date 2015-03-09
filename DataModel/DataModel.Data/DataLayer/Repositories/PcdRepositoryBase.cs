using System;
using Abp.Domain.Entities;
using RocketPos.Data.DataLayer;

namespace DataModel.Data.DataLayer.Repositories
{
    public abstract class PcdRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<DataContext, TEntity, TPrimaryKey>, IDisposable
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected PcdRepositoryBase()
            : base(new DataContext())
        {

        }

        private bool _disposed;

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed || !disposing)
                return;

            if (Context != null)
                Context.Dispose();

            _disposed = true;
        }
        #endregion

        
    }

    public abstract class PcdRepositoryBase<TEntity> : PcdRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {

        //do not add any method here, add to the class above (since this inherits it)
    }
}
