using Core.DTO;
using Core.Entites;
using Core.Enums;

namespace Business.Interfaces
{
    public interface IProductCategoryManager
    {
        Task Create(ProductCategory productCategury, CancellationToken cancellationToken = default);
        Task<Result> Delete(int productId, int categoryId, CancellationToken cancellationToken = default);
    }
}
