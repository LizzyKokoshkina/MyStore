using Autofac;
using Business.Interfaces;
using Core.Entites;
using Core.Enums;
using Microsoft.AspNetCore.Mvc;
using MyProject.Attributes;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Authorization;

namespace MyProject.Controllers
{
    [RouteApi("[controller]")]
    public class CategoryController : BaseApiController
    {
        #region Constructors 

        public CategoryController(ILifetimeScope scope): base(scope) { }

        #endregion

        #region Actions

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            return Ok(await Manager<ICategoryManager>().Get(cancellationToken));
        }

        [HttpPost, Policy(UserRole.Admin)]
        public async Task<IActionResult> Create([FromBody] Category category, CancellationToken cancellationToken = default)
        {
            return Ok(await Manager<ICategoryManager>().Create(category, cancellationToken));
        }

        [HttpPut, Policy(UserRole.Admin)]
        public async Task<IActionResult> Update([FromBody] Category category, CancellationToken cancellationToken = default)
        {
            return Ok(await Manager<ICategoryManager>().Update(category, cancellationToken));
        }


        [HttpDelete, Policy(UserRole.Admin)]
        public async Task<IActionResult> Delete([FromQuery] int id, CancellationToken cancellationToken = default)
        {
            return Ok(await Manager<ICategoryManager>().Delete(id, cancellationToken));
        }

        #endregion
    }
}
