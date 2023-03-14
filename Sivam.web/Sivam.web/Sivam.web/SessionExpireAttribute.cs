using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sivam.web
{
    public class SessionExpireAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            var skipAuthorization = filterContext.ActionDescriptor.IsDefined(
                   typeof(AllowAnonymousAttribute),
                   inherit: true);

            if (skipAuthorization)
            {

                return;
            }
            var controllername = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var actionname = filterContext.ActionDescriptor.ActionName;
            if (controllername.ToLower() == "Home".ToLower() && actionname.ToLower() == "GetOTP".ToLower())
            {
                if (HttpContext.Current.Session["validtOtpSe"]?.ToString() == "Valid")
                {
                    return;
                }
                filterContext.HttpContext.Response.StatusCode = 401;
                filterContext.Result = new Http401Result();

            }
            // check  sessions here            
            //if (HttpContext.Current.Session["Home"] == null)
            //{
            //if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            //{
            //    // filterContext.HttpContext.Response.StatusCode = 401;
            //    //filterContext.Result = new JsonResult { Data = "LogOut" };
            //    // filterContext.HttpContext.Response.End();
            //    filterContext.Result = new Http401Result();
            //}
            //else
            //{
            //    //filterContext.Result =
            //    //             new RedirectToRouteResult(
            //    //                 new RouteValueDictionary(new { controller = "Home", action = "Start" }));

            //    filterContext.Result = new RedirectResult("~/Home/Index");
            //    filterContext.Result = new RedirectResult("~/Home/Index");
            //    return;
            //}
            //}
            // base.OnActionExecuting(filterContext);
        }

        private class Http401Result : ActionResult
        {
            /// <summary>
            /// The execute result.
            /// </summary>
            /// <param name="context">
            /// The context.
            /// </param>
            public override void ExecuteResult(ControllerContext context)
            {
                context.HttpContext.Response.StatusCode = 401;
                context.HttpContext.Response.Write("AuthorizationLost Please LogOut And LogIn Again To Continue");
                context.HttpContext.Response.End();
            }
        }

    }
}