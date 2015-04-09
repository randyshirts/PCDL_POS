using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace PcdWeb.EntityFramework.Repositories
{
    public abstract class PcdWebRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<PcdWebDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected PcdWebRepositoryBase(IDbContextProvider<PcdWebDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class PcdWebRepositoryBase<TEntity> : PcdWebRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected PcdWebRepositoryBase(IDbContextProvider<PcdWebDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
