using Autofac;
using Business.Interfaces;
using Core.DTO;
using Core.Enums;
using Core.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Attributes;
using System.Threading;
using System.Threading.Tasks;

namespace MyProject.Controllers
{
    [RouteApi("[controller]")]
    public class OrderController : BaseApiController
    {
        #region Properties

        private IBraintreeConfiguration braintreeConfiguration;

        #endregion

        #region Constructors 

        public OrderController(ILifetimeScope scope,
            IBraintreeConfiguration braintreeConfiguration) : base(scope)
        {
            this.braintreeConfiguration = braintreeConfiguration;
        }

        #endregion

        #region Actions

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] NewOrderDto order, CancellationToken cancellationToken = default)
        {
            return Ok(await Manager<IOrderManager>().Create(order, cancellationToken));
        }

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> Get([FromQuery] int userId, CancellationToken cancellationToken = default)
        {
            return Ok(await Manager<IOrderManager>().Get(userId, cancellationToken));
        }

        [HttpGet("payment-token"), AllowAnonymous]
        public async Task<IActionResult> GetPaymentToken()
        {
            var token = await braintreeConfiguration.GetGateway().ClientToken.GenerateAsync();
            return Ok(new { token });
        }

        [HttpGet("transactions")]
        public IActionResult GetTransactions()
        {
            return Ok(Manager<IOrderManager>().GetTransactions());
        }


        [HttpPut("refund")]
        public async Task<IActionResult> Refund([FromQuery] int orderId, CancellationToken cancellationToken = default)
        {
            return Ok(await Manager<IOrderManager>().Refund(orderId, cancellationToken));
        }

        #endregion
    }
}
