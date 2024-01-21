using Core.Entites;
using Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Database.Repository
{
    public class MySqlRepository<TEntity> : IMySqlRepository<TEntity> where TEntity : class, ISqlEntity
    {
        protected readonly DbContext dbContext;
        protected readonly DbSet<TEntity> dbSet;

        public MySqlRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<TEntity>();
        }

        public async Task<TEntity> Create(TEntity entity, CancellationToken cancellationToken = default)
        {
            entity.CleanNavigationProperties();
            await dbSet.AddAsync(entity, cancellationToken);
            return entity;
        }

        public Task<TEntity> Update(TEntity entity, CancellationToken cancellationToken = default)
        {
            entity.CleanNavigationProperties();
            dbSet.Update(entity);
            return Task.FromResult(entity);
        }

        public Task Delete(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default)
        {
            var entites = dbSet.Where(condition);
            dbSet.RemoveRange(entites);
            return Task.CompletedTask;
        }

        public Task<bool> Exist(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default)
        {
            return dbSet.AnyAsync(condition, cancellationToken);
        }

        public Task<IEnumerable<TResult>> Get<TResult>(Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(dbSet.Where(condition).Select(selector).AsEnumerable());
        }

        public Task<TResult> GetFirst<TResult>(Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default)
        {
            return dbSet.Where(condition).Select(selector).FirstOrDefaultAsync(cancellationToken);
        }

        public void Dispose()
        {
            dbContext?.Dispose();
        }
    }
}
