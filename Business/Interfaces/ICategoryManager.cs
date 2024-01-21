using Core.Entites;
using Core.Enums;

namespace Business.Interfaces
{
    public interface ICategoryManager
    {
        Task<IEnumerable<Category>> Get(CancellationToken cancellationToken = default);
        Task<Category> Create(Category category, CancellationToken cancellationToken = default);
        Task<Result> Update(Category category, CancellationToken cancellationToken = default);
        Task<Result> Delete(int id, CancellationToken cancellationToken = default);

    }
}
