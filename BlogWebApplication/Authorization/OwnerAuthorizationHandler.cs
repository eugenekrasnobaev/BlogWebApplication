using BlogBLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

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
