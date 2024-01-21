using Core.Database.Repository;
using Core.Entites;

namespace Core.Database.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task BeginTransaction(CancellationToken cancellationToken = default);
        Task CommitTransaction(CancellationToken cancellationToken = default);
        Task SaveChanges(CancellationToken cancellationToken = default);
        IMySqlRepository<TEntity> Sql<TEntity>() where TEntity : class, ISqlEntity;
    }
}
