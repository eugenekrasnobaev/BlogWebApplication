using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
    
