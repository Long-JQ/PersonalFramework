using Model;
using PersonalFramework.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        string LoginOnlyAdmin = ConfigurationManager.AppSettings["LoginOnlyAdmin"];

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
                var account = AdminLoginHelper.AdminLogin(keyword, password) != null ? 1 :0 ;
                if (LoginOnlyAdmin == "false" && account==0)
                {
                    account = UserLoginHelper.UserLogin(keyword, password) != null ? 1 : 0;
                }
                //var account = AdminLoginHelper.AdminLogin(keyword, password);
                if (account == 0)
                {
                    ReturnData result = new ReturnData(500,"登录失败");
                    return result.ToJson();
                }
                else
                {
                    ReturnData result = new ReturnData(0,"登录成功");
                    var admin = PersonalFramework.Service.AdminLoginHelper.CurrentUser();
                    //记录操作日志，写进操作日志中
                    var log = new ActionLog();
                    log.ActionContent = "管理员登录";
                    log.CreateTime = DateTime.Now;
                    log.IP = IPHelper.GetClientIp();
                    log.Location = "Login/Login";
                    log.RequestData = Request.Form.ToString();
                    log.Platform = "后台";
                    log.Source = Request.HttpMethod;
                    log.RequestUrl = Request.Url.AbsoluteUri;
                    if (admin != null)
                    {
                        log.UID = admin.ID;
                        log.UserName = admin.AdminName;
                    }
                    DataContext context = new DataContext();
                    context.ActionLog.Add(log);
                    context.SaveChanges();
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
