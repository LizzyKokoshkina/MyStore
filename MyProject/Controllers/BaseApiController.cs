using Autofac;
using Microsoft.AspNetCore.Mvc;

namespace MyProject.Controllers
{
    public class BaseApiController : Controller
    {
        protected ILifetimeScope Scope { get; private set; }

        protected T Manager<T>()
        {
            return Scope.Resolve<T>();
        }

        public BaseApiController(ILifetimeScope scope) 
        {
            Scope = scope;
        }

        protected override void Dispose(bool disposing)
        {
            Scope?.Dispose();
            base.Dispose(disposing);
        }

    }
}
