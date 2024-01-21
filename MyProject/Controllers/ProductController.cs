using Autofac;
using Business.Interfaces;
using Core.Entites;
using Microsoft.AspNetCore.Mvc;
using MyProject.Attributes;
using System.Threading.Tasks;
using System.Threading;
using Core.Enums;
using Core.DTO;
using Microsoft.AspNetCore.Authorization;

namespace MyProject.Controllers
{
    [RouteApi("[controller]")]
    public class ProductController : BaseApiController
    {
        #region Constructors 

        public ProductController(ILifetimeScope scope): base(scope) { }

        #endregion

        #region Actions

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            return Ok(await Manager<IProductManager>().Get(cancellationToken));
        }

        [HttpGet("category"), AllowAnonymous]
        public async Task<IActionResult> GetByCategory([FromQuery] int categoryId, CancellationToken cancellationToken)
        {
            return Ok(await Manager<IProductManager>().GetByCategory(categoryId, cancellationToken));
        }

        [HttpGet("product"), AllowAnonymous]
        public async Task<IActionResult> GetProduct([FromQuery] int productId, CancellationToken cancellationToken)
        {
            return Ok(await Manager<IProductManager>().GetProduct(productId, cancellationToken));
        }

        [HttpPost, Policy(UserRole.Admin)]
        public async Task<IActionResult> Create([FromBody] Product product, CancellationToken cancellationToken = default)
        {
            return Ok(await Manager<IProductManager>().Create(product, cancellationToken));
        }

        [HttpPut, Policy(UserRole.Admin)]
        public async Task<IActionResult> Update([FromBody] Product product, CancellationToken cancellationToken = default)
        {
            return Ok(await Manager<IProductManager>().Update(product, cancellationToken));
        }


        [HttpDelete, Policy(UserRole.Admin)]
        public async Task<IActionResult> Delete([FromQuery] int id, CancellationToken cancellationToken = default)
        {
            return Ok(await Manager<IProductManager>().Delete(id, cancellationToken));
        }


        #endregion
    }
}
