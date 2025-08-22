using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyApi5.UI.Filters
{
    public class AuthFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
           
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as ControllerBase;
            if (context.HttpContext.Request.Cookies["token"] == null)
            {
                context.Result = controller.RedirectToAction(
                    actionName: "Login",
                    controllerName: "Account",
                    //new { message = "You are not allowed to register, since the company data not exist!" }
                    new {requestUrl = context.HttpContext.Request.Path}
                    );
            }
        }
    }
}
