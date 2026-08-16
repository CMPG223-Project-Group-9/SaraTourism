using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(SaraTourism.Filters.RequireAdminAttribute))]

namespace SaraTourism.Filters
{
    /// <summary>Restricts an action/controller to logged-in admins.</summary>
    public class RequireAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            if (session == null || session["AdminID"] == null)
            {
                filterContext.Result = new RedirectResult("~/Account/AdminLogin");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
