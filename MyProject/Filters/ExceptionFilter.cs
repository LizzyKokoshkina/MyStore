using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MyProject.Filters
{
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            return Task.Run(() =>
            {
                if (context.Exception is ValidationException)
                {
                    context.Result = new BadRequestObjectResult(new
                    {
                        Errors = new[] { new { ErrorMessage = context.Exception.Message, ErrorCode = context.Exception.HResult } }
                    });
                    context.ExceptionHandled = true;
                    return;
                }
                if(context.Exception is Exception)
                {
                    context.Result = new ObjectResult(new
                    {
                        Errors = new[] { new { ErrorMessage = "Internal Server Error", ErrorCode = 500, context.Exception } }
                    });
                    context.ExceptionHandled = true;
                    return;
                }
            });
        }
    }
}
