using Autofac;
using Business.Interfaces;
using Core.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Attributes;
using System.Threading;
using System.Threading.Tasks;

namespace MyProject.Controllers
{
    [RouteApi("[controller]")]
    public class AccountController : BaseApiController
    {
        #region Constructors 

        public AccountController(ILifetimeScope scope): base(scope) { }

        #endregion

        #region Actions

        [HttpPost("login"), AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto login, CancellationToken cancellationToken = default)
        {
            return Ok(await Manager<IAccountManager>().Login(login, cancellationToken));
        }

        [HttpPost("register"), AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto register, CancellationToken cancellationToken = default)
        {
            return Ok(await Manager<IAccountManager>().Register(register, cancellationToken));
        }

        #endregion
    }
}
