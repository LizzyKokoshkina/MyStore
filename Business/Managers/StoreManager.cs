using Business.Interfaces;
using Core.Database.UnitOfWork;
using Core.Entites;
using Core.Enums;

namespace Business.Managers
{
    public class StoreManager : ManagerBase, IStoreManager
    {
        const int NewProducsCount = 8;

        public StoreManager(UnitOfWork unitOfWork) : base(unitOfWork) { }

        public Task<IEnumerable<Product>> GetAll(CancellationToken cancellationToken = default)
        {
            return UnitOfWork.Sql<Product>().Get(x => true, x => x, cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetNewProducts(CancellationToken cancellationToken = default)
        {
            return (await UnitOfWork.Sql<Product>().Get(x => true, x => x, cancellationToken))
                .OrderByDescending(x => x.DateAdded).Take(NewProducsCount);
        }

        public Task<Product> GetProduct(int id, CancellationToken cancellationToken = default)
        {
            return UnitOfWork.Sql<Product>().GetFirst(x => x.Id == id, x => x, cancellationToken);
        }

        public async Task<Product> Create(Product product, CancellationToken cancellationToken = default)
        {
            var entity = await UnitOfWork.Sql<Product>().Create(product, cancellationToken);
            await UnitOfWork.SaveChanges(cancellationToken);
            return entity;
        }

        public async Task<Result> Update(Product product, CancellationToken cancellationToken = default)
        {
            var entity = await UnitOfWork.Sql<Product>().GetFirst(x => x.Id == product.Id, x => x, cancellationToken);
            if (entity != null)
            {
                entity.Title = product.Title;
                entity.Description = product.Description;
                entity.Price = product.Price;
                entity.NewPrice = product.NewPrice;
                entity.IsOnSale = product.IsOnSale;
                entity.Color = product.Color;
                entity.Sizes = product.Sizes;
                entity.Quantity = product.Quantity;
                UnitOfWork.Sql<Product>().Update(entity);
                await UnitOfWork.SaveChanges(cancellationToken);
                return Result.Success;
            }
            return Result.Fail;
        }

        public async Task<Result> Delete(int id, CancellationToken cancellationToken = default)
        {
            await UnitOfWork.Sql<Product>().Delete(x => x.Id == id, cancellationToken);
            await UnitOfWork.SaveChanges(cancellationToken);
            return Result.Success;
        }
    }
}
