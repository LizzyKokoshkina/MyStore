using Core.Database.UnitOfWork;

namespace Business.Managers
{
    public abstract class ManagerBase : IDisposable
    {
        protected IUnitOfWork UnitOfWork { get; }

        protected ManagerBase(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            UnitOfWork?.Dispose();
        }
    }
}
