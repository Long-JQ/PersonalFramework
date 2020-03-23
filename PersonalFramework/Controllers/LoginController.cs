using PersonalFramework.Tool;
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
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            string token = "";
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                token = cookie.Value;
            }
            else
            {
                var headers = System.Web.HttpContext.Current.Request.Headers["Authorization"];
                if (!string.IsNullOrEmpty(headers))
                {
                    token = headers;
                }
            }
            
            var currentUser = LoginHelper.GetUser(token);
            if (currentUser == null)
            {

            }
        }
        public ActionResult Index()
        {
            return View();
        }
        public string Login(string keyword, string password)
        {
            var userService = new Context.DataContext();
            var account = userService.Users.First(a => a.UserName == keyword.Trim());
            if (account != null)
            {
                var result = DeCrypt.VerifyPassWord(account.Password, password, account.Salt);
                if (!result)
                {
                    return null;
                }
                string token = Guid.NewGuid().ToString("N");
                Session[token] = account;
                var EncryptToken = LoginHelper.Login(account.Token);
                return EncryptToken;
            }
            else
            {
                return null;
            }
        }
    }
}
