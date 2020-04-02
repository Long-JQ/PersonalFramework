using Model;
using PersonalFramework.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;

namespace PersonalFramework.Controllers
{
    public class LoginController : Controller
    {
        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    base.OnActionExecuting(filterContext);

            
        //    var currentUser = LoginHelper.CurrentUser();
        //    if (currentUser == null)
        //    {

        //    }
        //}
        public ActionResult Index()
        {
            return View();
        }
        public string Login(string keyword, string password)
        {
            try
            {
                var account = LoginHelper.UserLogin(keyword, password);
                if (account == null)
                {
                    ReturnData result = new ReturnData(500,"登录失败");
                    return result.ToJson();
                }
                else
                {
                    ReturnData result = new ReturnData(0,"登录成功");
                    return result.ToJson();
                }
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }
        [System.ComponentModel.DescriptionAttribute("啊飒飒大王的")]
        /// <summary>
        /// 啊飒飒大王的
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginOut()
        {
            LoginHelper.UserLogout();
            return RedirectToAction("index");
        }
    }
}
