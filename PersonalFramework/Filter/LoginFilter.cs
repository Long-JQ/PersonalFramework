using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalFramework.Filter
{
    public class LoginFilter : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            else
            {
                string path = context.HttpContext.Request.Path;
                string strUrl = "/Account/LogOn?returnUrl={0}";

                context.HttpContext.Response.Redirect(string.Format(strUrl, HttpUtility.UrlEncode(path)), true);
            }
        }
    }
}