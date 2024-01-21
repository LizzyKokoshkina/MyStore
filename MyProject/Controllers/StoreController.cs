using Autofac;
using MyProject.Attributes;

namespace MyProject.Controllers
{
    [RouteApi("[controller]")]
    public class StoreController : BaseApiController
    {
        #region Constructors 

        public StoreController(ILifetimeScope scope): base(scope) { }

        #endregion

        #region Actions



        #endregion
    }
}
