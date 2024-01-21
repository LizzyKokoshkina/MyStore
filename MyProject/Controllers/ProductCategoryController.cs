using Autofac;
using Business.Interfaces;
using Core.Entites;
using Core.Enums;
using Microsoft.AspNetCore.Mvc;
using MyProject.Attributes;
using System.Threading.Tasks;
using System.Threading;

namespace MyProject.Controllers
{
    [RouteApi("[controller]")]
    public class ProductCategoryController : BaseApiController
    {
        #region Constructors 

        public ProductCategoryController(ILifetimeScope scope): base(scope) { }

        #endregion

        #region Actions
                
        [HttpPost, Policy(UserRole.Admin)]
        public async Task<IActionResult> Create([FromBody] ProductCategory productCategory, CancellationToken cancellationToken = default)
        {
            await Manager<IProductCategoryManager>().Create(productCategory, cancellationToken);
            return Ok();
        }

        [HttpDelete, Policy(UserRole.Admin)]
        public async Task<IActionResult> Delete([FromQuery] int productId, [FromQuery] int categoryId, CancellationToken cancellationToken = default)
        {
            return Ok(await Manager<IProductCategoryManager>().Delete(productId, categoryId, cancellationToken));
        }

        #endregion
    }
}
