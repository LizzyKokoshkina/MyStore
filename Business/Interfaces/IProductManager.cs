using Core.DTO;
using Core.Entites;
using Core.Enums;

namespace Business.Interfaces
{
    public interface IProductManager
    {
        Task<IEnumerable<ProductDto>> Get(CancellationToken cancellationToken = default);
        Task<CategoryDto> GetByCategory(int categoryId, CancellationToken cancellationToken = default);
        Task<ProductDto> GetProduct(int productId, CancellationToken cancellationToken = default);
        Task<Product> Create(Product product, CancellationToken cancellationToken = default);
        Task<Result> Update(Product product, CancellationToken cancellationToken = default);
        Task<Result> Delete(int id, CancellationToken cancellationToken = default);
    }
}
