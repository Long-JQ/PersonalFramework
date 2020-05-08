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
        [System.ComponentModel.DescriptionAttribute("登录页")]
        public ActionResult Index()
        {
            return View();
        }
        [System.ComponentModel.DescriptionAttribute("登录")]
        public string Login(string keyword, string password)
        {
            try
            {
                var account = UserLoginHelper.UserLogin(keyword, password);
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
                ReturnData result = new ReturnData(500, ex.Message);
                return result.ToJson();
            }
        }
        [System.ComponentModel.DescriptionAttribute("退出登录")]
        /// <summary>
        /// 啊飒飒大王的
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginOut()
        {
            UserLoginHelper.UserLogout();
            return RedirectToAction("index");
        }
    }
}
