using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BlogWebApplication.Filters
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (!(context.Exception is ArgumentException))
            {
                context.ExceptionHandled = false;
                return;
            }

            var exceptionMessage = context.Exception.Message;

            context.Result = new ContentResult
            {
                Content = $"Error! {exceptionMessage}"
            };

            context.ExceptionHandled = true;
        }
    }
}
    
