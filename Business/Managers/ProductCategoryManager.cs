using Business.Interfaces;
using Core.Database.UnitOfWork;
using Core.Entites;
using Core.Enums;

namespace Business.Managers
{
    public class ProductCategoryManager : ManagerBase, IProductCategoryManager
    {
        public ProductCategoryManager(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task Create(ProductCategory productCategory, CancellationToken cancellationToken = default)
        {
            await UnitOfWork.Sql<ProductCategory>().Create(productCategory, cancellationToken);
            await UnitOfWork.SaveChanges(cancellationToken);
        }

        public async Task<Result> Delete(int productId, int categoryId, CancellationToken cancellationToken = default)
        {
            await UnitOfWork.Sql<ProductCategory>().Delete(x => x.ProductId == productId && x.CategoryId == categoryId, cancellationToken);
            await UnitOfWork.SaveChanges(cancellationToken);
            return Result.Success;
        }

    }
}
