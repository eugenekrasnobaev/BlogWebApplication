using BlogBLL.Interfaces;
using BlogWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogDAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogWebApplication.Authorization
{
    public class OwnerAuthorizationHandler : AuthorizationHandler<OwnerRequirement, int>
    {
        private readonly IAccountService accountService;
        public OwnerAuthorizationHandler(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       OwnerRequirement requirement,
                                                       int userId)
        {
            var user = accountService.GetUserById(userId);

            if (context.User.Identity.Name == user.Name || context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class OwnerRequirement : IAuthorizationRequirement { }
}
