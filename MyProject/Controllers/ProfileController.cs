using Autofac;
using Business.Interfaces;
using Core.DTO;
using Core.Entites;
using Microsoft.AspNetCore.Mvc;
using MyProject.Attributes;
using System.Threading;
using System.Threading.Tasks;

namespace MyProject.Controllers
{
    [RouteApi("[controller]")]
    public class ProfileController : BaseApiController
    {
        #region Constructors 

        public ProfileController(ILifetimeScope scope): base(scope) { }

        #endregion

        #region Actions

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] User user, CancellationToken cancellationToken = default)
        {
            return Ok(await Manager<IProfileManager>().Edit(user, cancellationToken));
        }

        [HttpPut("password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto password, CancellationToken cancellationToken = default)
        {
            return Ok(await Manager<IProfileManager>().ChangePassword(password, cancellationToken));
        }

        #endregion
    }
}
