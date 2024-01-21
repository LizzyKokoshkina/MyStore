using Business.Interfaces;
using Core.Database.UnitOfWork;
using Core.Entites;
using Core.Enums;

namespace Business.Managers
{
    public class CategoryManager : ManagerBase, ICategoryManager
    {
        public CategoryManager(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public Task<IEnumerable<Category>> Get(CancellationToken cancellationToken = default)
        {
            return UnitOfWork.Sql<Category>().Get(x => true, x => x, cancellationToken);
        }

        public async Task<Category> Create(Category category, CancellationToken cancellationToken = default)
        {
            category.Image = "abc";
            var entity = await UnitOfWork.Sql<Category>().Create(category, cancellationToken);
            await UnitOfWork.SaveChanges(cancellationToken);
            return entity;
        }

        public async Task<Result> Update(Category category, CancellationToken cancellationToken = default)
        {
            var entity = await UnitOfWork.Sql<Category>().GetFirst(x => x.Id == category.Id, x => x, cancellationToken);
            if (entity != null)
            {
                entity.Name = category.Name;
                entity.Image = "abc";
                await UnitOfWork.Sql<Category>().Update(entity);
                await UnitOfWork.SaveChanges(cancellationToken);
                return Result.Success;
            }
            return Result.Fail;
        }

        public async Task<Result> Delete(int id, CancellationToken cancellationToken = default)
        {
            await UnitOfWork.Sql<Category>().Delete(x => x.Id == id, cancellationToken);
            await UnitOfWork.SaveChanges(cancellationToken);
            return Result.Success;
        }

    }
}
