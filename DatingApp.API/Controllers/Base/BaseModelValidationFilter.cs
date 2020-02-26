using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DatingApp.API.Controllers.Base
{
    public class BaseModelValidationFilter : ActionFilterAttribute
    {
        public override object TypeId => base.TypeId;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool IsDefaultAttribute()
        {
            return base.IsDefaultAttribute();
        }

        public override bool Match(object obj)
        {
            return base.Match(obj);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            return base.OnActionExecutionAsync(context, next);
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
        }

        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is BadRequestObjectResult bar)
            {
                if (bar.Value is ValidationProblemDetails vpd)
                {
                    Console.WriteLine(vpd);
                    bar.Value = new
                    {
                        data = (string)null,
                        message = string.Join(Environment.NewLine, vpd.Errors?.Select(c => $"{c.Key}: {string.Join(Environment.NewLine,c.Value)}"))
                    };
                }
            }
            return base.OnResultExecutionAsync(context, next);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}