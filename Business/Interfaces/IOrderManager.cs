using Core.DTO;
using Core.Entites;
using Core.Enums;

namespace Business.Interfaces
{
    public interface IOrderManager
    {
        Task<Result> Create(NewOrderDto order, CancellationToken cancellationToken = default);
        Task<IEnumerable<Order>> Get(int userId, CancellationToken cancellationToken = default);
        IEnumerable<TransactionDto> GetTransactions();
        Task<Result> Refund(int orderId, CancellationToken cancellationToken = default);
    }
}
