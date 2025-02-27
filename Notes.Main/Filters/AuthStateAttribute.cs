using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Notes.Main.Filters
{
    public class AuthStateAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var userIdentity = filterContext.HttpContext.User.Identity;
            if (userIdentity != null)
            {
                ((Controller)filterContext.Controller).ViewData["IsAuth"] = userIdentity.IsAuthenticated;
                ((Controller)filterContext.Controller).ViewData["UserName"] = userIdentity.Name;
                ((Controller)filterContext.Controller).ViewData["UserId"] = userIdentity.Name;
            }
        }
    }
}
