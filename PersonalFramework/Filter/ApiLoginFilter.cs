using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalFramework
{
    public class ApiLoginFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            //校验用户是否已经登录
            var model = PersonalFramework.Service.UserLoginHelper.CurrentUser();
            if (model != null)
            {
                filterContext.HttpContext.Response.Write("请登录");
            }
        }
    }
}