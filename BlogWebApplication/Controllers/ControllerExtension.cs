using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogWebApplication.Controllers
{
    public static class ControllerExtension
    {
        public static string CurrentUserName(this Controller controller)
        {
            return controller.User.FindFirstValue(ClaimTypes.Name);
        }

        public static string CurrentUserEmail(this Controller controller)
        {
            return controller.User.FindFirstValue(ClaimTypes.Email);
        }

        public static bool IsAuthenticated(this Controller controller)
        {
            return controller.User.Identity.IsAuthenticated;
        }
    }
}
