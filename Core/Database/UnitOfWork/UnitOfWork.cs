using Autofac;
using Core.Database.Repository;
using Core.Entites;
using Microsoft.EntityFrameworkCore;

namespace Core.Database.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext dbContext;
        private readonly ILifetimeScope lifetimeScope;

        public UnitOfWork(DbContext context, ILifetimeScope scope)
        {
            dbContext = context;
            lifetimeScope = scope;
        }

        public Task BeginTransaction(CancellationToken cancellationToken = default)
        {
            return dbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public Task CommitTransaction(CancellationToken cancellationToken = default)
        {
            if (dbContext.Database.CurrentTransaction != null)
            {
                return dbContext.Database.CommitTransactionAsync();
            }
            return Task.CompletedTask;
        }

        public async Task SaveChanges(CancellationToken cancellationToken = default)
        {
            try
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                if (dbContext.Database.CurrentTransaction != null)
                {
                    await dbContext.Database.RollbackTransactionAsync(cancellationToken);
                }
                throw;
            }
        }

        public IMySqlRepository<TEntity> Sql<TEntity>() where TEntity : class, ISqlEntity
        {
            return lifetimeScope.Resolve<IMySqlRepository<TEntity>>(new NamedParameter("context", dbContext));
        }

        public void Dispose()
        {
            dbContext?.Dispose();
        }
    }
}
