using Core.Entites;
using System.Linq.Expressions;

namespace Core.Database.Repository
{
    public interface IMySqlRepository<TEntity> : IDisposable where TEntity : class, ISqlEntity
    {
        Task<IEnumerable<TResult>> Get<TResult>(Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default);
        Task<TResult> GetFirst<TResult>(Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default);
        Task<TEntity> Create(TEntity entity, CancellationToken cancellationToken = default);
        public Task<TEntity> Update(TEntity entity, CancellationToken cancellationToken = default);
        Task Delete(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default);
        Task<bool> Exist(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default);
    }
}
