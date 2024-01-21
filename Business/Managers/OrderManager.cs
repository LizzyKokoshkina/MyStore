using Braintree;
using Business.Interfaces;
using Business.Services;
using Core.Database.UnitOfWork;
using Core.DTO;
using Core.Entites;
using Core.Enums;
using Core.Payments;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Business.Managers
{
    public class OrderManager : ManagerBase, IOrderManager
    {
        private readonly IEmailService emailService;
        private readonly IBraintreeConfiguration braintreeConfiguration;

        public OrderManager(IUnitOfWork unitOfWork,
            IEmailService emailService,
            IBraintreeConfiguration braintreeConfiguration) : base(unitOfWork)
        {
            this.emailService = emailService;
            this.braintreeConfiguration = braintreeConfiguration;
        }

        public async Task<Result> Create(NewOrderDto order, CancellationToken cancellationToken = default)
        {
            var entity = await UnitOfWork.Sql<Order>().Create(new Order
            {
                Date = DateTime.UtcNow,
                Total = order.Items.Sum(x => x.Price * 1),
                //Total = order.Items.Sum(x => x.Price * x.Quantity),
                UserId = order.UserId,
                Status = string.IsNullOrEmpty(order.Nonce) ? "New" : "Paid",
                Address = order.Address,
                City = order.City,
                Email = order.Email,
                Firstname = order.Firstname,
                Lastname = order.Lastname,
                Phone = order.Phone,
                Nonce = order.Nonce,
            }, cancellationToken);
            await UnitOfWork.SaveChanges(cancellationToken);

            foreach (var item in order.Items)
            {
                await UnitOfWork.Sql<OrderItem>().Create(new OrderItem
                {
                    OrderId = entity.Id,
                    Description = item.Description,
                    Title = item.Title,
                    Price = item.Price
                });
                var product = await UnitOfWork.Sql<Product>().GetFirst(x => x.Id == item.Id, x => x, cancellationToken);
                product.Quantity = product.Quantity - item.Quantity;
                await UnitOfWork.Sql<Product>().Update(product, cancellationToken);
            }

            await UnitOfWork.SaveChanges(cancellationToken);

            if (!string.IsNullOrEmpty(order.Nonce))
            {
                var gateway = braintreeConfiguration.GetGateway();
                var request = new TransactionRequest
                {
                    PurchaseOrderNumber = entity.Id.ToString(),
                    Amount = Convert.ToDecimal(entity.Total),
                    PaymentMethodNonce = order.Nonce,
                    Customer = new CustomerRequest
                    {
                        FirstName = order.Firstname,
                        LastName = order.Lastname,
                        Email = order.Email,
                        Company = order.City
                    },
                    Options = new TransactionOptionsRequest
                    {
                        SubmitForSettlement = true,
                    }
                };

                var result = gateway.Transaction.Sale(request);
            }

            return Result.Success;
        }

        public Task<IEnumerable<Order>> Get(int userId, CancellationToken cancellationToken = default)
        {
            return UnitOfWork.Sql<Order>().Get(x => x.UserId == userId, x => new Order
            {
                Id = x.Id,
                Status = x.Status,
                Date = x.Date,
                Total = x.Total,
                Items = x.Items.Select(x => new OrderItem
                {
                    Title = x.Title,
                    Price = x.Price
                }).ToList(),
            }, cancellationToken);
        }

        public IEnumerable<TransactionDto> GetTransactions()
        {
            var request = new TransactionSearchRequest();
            var gateway = braintreeConfiguration.GetGateway();
            ResourceCollection<Transaction> collection = gateway.Transaction.Search(request);
            var transactions = new List<TransactionDto>();

            foreach (Transaction transaction in collection)
            {
                transactions.Add(new TransactionDto
                {
                    Status = transaction.Status.ToString(),
                    CreatedAt = transaction.CreatedAt,
                    Email = transaction.CustomerDetails.Email,
                    Firstname = transaction.CustomerDetails.FirstName,
                    Lastname = transaction.CustomerDetails.LastName,
                    Amount = transaction.Amount
                });
            }

            return transactions;
        }

        public async Task<Result> Refund(int orderId, CancellationToken cancellationToken = default)
        {
            var order = await UnitOfWork.Sql<Order>().GetFirst(x => x.Id == orderId, x => x, cancellationToken);
            if (order == null || order.Date.AddDays(14) <= DateTime.UtcNow)
            {
                return Result.Fail;
            }
            if (order.Status == "Refunded")
            {
                return Result.Success;
            }
            var gateway = braintreeConfiguration.GetGateway();
            ResourceCollection<Transaction> collection = gateway.Transaction.Search(new TransactionSearchRequest());
            var orderTransaction = collection.FirstOrDefault(x => x.PurchaseOrderNumber == order.Id.ToString());
            if(orderTransaction == null)
            {
                return Result.Fail;
            }
            try
            {
                await gateway.Transaction.RefundAsync(orderTransaction.Id);
            }
            catch
            {
                return Result.Fail;
            }
           
            order.Status = "Refunded";
            await UnitOfWork.Sql<Order>().Update(order, cancellationToken);
            await UnitOfWork.SaveChanges(cancellationToken);
            return Result.Success;
        }
    }
}
