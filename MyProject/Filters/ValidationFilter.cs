using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace MyProject.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(new
                {
                    Errors = context.ModelState.SelectMany(i => i.Value.Errors.Select(e => new
                    {
                        Property = string.IsNullOrEmpty(i.Key) ? string.Empty : $"{char.ToLowerInvariant(i.Key[0])}{i.Key.Substring(1)}",
                        e.ErrorMessage,
                        ErrorCode = 3
                    }))
                });
            } else
            {
                await next();
            }
        }
    }
}
