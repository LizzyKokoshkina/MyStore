using Core.Entites;
using Core.Enums;

namespace Business.Interfaces
{
    public interface IStoreManager
    {
        Task<IEnumerable<Product>> GetAll(CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetNewProducts(CancellationToken cancellationToken = default);
        Task<Product> GetProduct(int id, CancellationToken cancellationToken = default);
        Task<Product> Create(Product product, CancellationToken cancellationToken = default);
        Task<Result> Update(Product product, CancellationToken cancellationToken = default);
        Task<Result> Delete(int id, CancellationToken cancellationToken = default);
    }
}
