using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DatingApp.API.Filters
{
    public class ResponseFilter : ActionFilterAttribute
    {
        public override object TypeId => base.TypeId;
        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is BadRequestObjectResult bar)
            {
                if (bar.Value is ValidationProblemDetails vpd)
                {
                    bar.Value = new BaseResponseModel()
                    {
                        Data = vpd.Errors,
                        Message = string.Join(Environment.NewLine, vpd.Errors?.Select(c => $"{c.Key}: {string.Join(Environment.NewLine, c.Value)}"))
                    };
                }
                else
                {
                    bar.Value = new BaseResponseModel()
                    {
                        Data = bar.Value,
                        Message = string.Empty
                    };
                }
            }
            else
            {
                if (context.Result is ObjectResult or)
                {
                    or.Value = new BaseResponseModel()
                    {
                        Data = or.Value,
                        Message = or.Value is ProblemDetails pd ? pd.Title : string.Empty
                    };
                }


            }
            return base.OnResultExecutionAsync(context, next);
        }
    }
}