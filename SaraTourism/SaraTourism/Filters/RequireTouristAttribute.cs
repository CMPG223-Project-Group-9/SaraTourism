using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(SaraTourism.Filters.RequireTouristAttribute))]

namespace SaraTourism.Filters
{
    /// <summary>
    /// Restricts an action/controller to logged-in tourists. Session-based (not ASP.NET
    /// Identity) to keep authentication simple and independent of the MySQL-backed
    /// stored-procedure data layer. Redirects to the login page with a return URL.
    /// </summary>
    public class RequireTouristAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            if (session == null || session["TouristID"] == null)
            {
                var returnUrl = filterContext.HttpContext.Request.RawUrl;
                filterContext.Result = new RedirectResult("~/Account/Login?returnUrl=" + System.Uri.EscapeDataString(returnUrl));
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
