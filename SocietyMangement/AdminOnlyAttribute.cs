using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class AdminOnlyAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var userRole = context.HttpContext.Session.GetString("Role");

        // If user is not an Admin, redirect to login page
        if (string.IsNullOrEmpty(userRole) || userRole != "Admin")
        {
            context.Result = new RedirectToActionResult("Login", "User", null);
        }

        base.OnActionExecuting(context);
    }
}
