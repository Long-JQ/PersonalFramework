using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
//using System.Web.Mvc;
using System.Web.Security;

namespace PersonalFramework.Service
{
    public class AdminLoginHelper
    {
        static User _loginuser = null;
        static string cookieName = "PersonalFrameworkPassword_Admin".ToMD5();
        static string tokenName = "PersonalFrameworkPassword_Token".ToMD5();

        public static Admin AdminLogin(string keyword, string password)
        {
            var context = new DataContext();
            var account = context.Admins.Where(x => x.AdminName == keyword.Trim()).FirstOrDefault();
            if (account != null)
            {
                var result = DeCrypt.VerifyPassWord(password, account.Password, account.Salt);
                if (!result)
                {
                    return null;
                }
                HttpContext.Current.Session[cookieName] = account;

                HttpCookie cookie = new HttpCookie(cookieName, account.AdminName);
                HttpContext.Current.Response.Cookies.Add(cookie);
                HttpCookie cookie2 = new HttpCookie(tokenName, account.AdminName.ToMD5()+account.Password);
                HttpContext.Current.Response.Cookies.Add(cookie2);

                return account;
            }
            else
            {
                return null;
            }
        }
        public static string Login(string token)
        {

            #region 存入 Cookie 票据

            // 设置Ticket信息
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                 1,
                token,
                DateTime.Now,
                DateTime.Now.AddDays(1),
                false,
                Service.IPHelper.GetClientIp()

                );

            // 加密验证票据
            string strTicket = FormsAuthentication.Encrypt(ticket);

            // 使用新userdata保存cookie
            HttpCookie cookie = new HttpCookie(cookieName, strTicket);
            //cookie.Expires = DateTime.Now.AddMinutes(30);
            cookie.Path = "/";

            HttpContext.Current.Response.Cookies.Add(cookie);
            return strTicket;
            #endregion
        }
        
        //登出
        /// <summary>
        /// 登出
        /// </summary>
        public static void UserLogout()
        {
            if (CurrentUser() !=null)
            {
                //获取会员ID
                var id = HttpContext.Current.User.Identity.Name;
                FormsAuthentication.SignOut();
                _loginuser = null;
                RemoveUser(id);
                HttpContext.Current.Session.Remove(tokenName);
            }
        }
        //移除指定会员ID的登录缓存
        /// <summary>
        /// 移除指定会员ID的登录缓存
        /// </summary>
        /// <param name="ID"></param>
        public static void RemoveUser(string ID)
        {
            HttpCookie cookie = new HttpCookie(cookieName, "");
            cookie.Expires = DateTime.Now.AddMinutes(-30);
            cookie.Path = "/";
            HttpContext.Current.Response.Cookies.Add(cookie);
            FormsAuthentication.SignOut();
        }
        //获取当前会员登录对象
        /// <summary>
        /// 获取当前会员登录对象
        /// <para>当没登陆或者登录信息不符时，这里返回 null </para>
        /// </summary>
        /// <returns></returns>
        public static Model.Admin CurrentUser()
        {
            //校验用户是否已经登录
            var user = HttpContext.Current.Session[cookieName] as Model.Admin;
            if (user != null) return user;
            else
            {
                if (HttpContext.Current.Request.Cookies[cookieName] != null && HttpContext.Current.Request.Cookies[tokenName] != null)
                {
                    string keyword = HttpContext.Current.Request.Cookies[cookieName].Value;
                    string token = HttpContext.Current.Request.Cookies[tokenName].Value;
                    string pwd = token.Substring(32);
                    DataContext context = new DataContext();
                    var account = context.Admins.Single(a => a.AdminName == keyword.Trim() && a.Password == pwd);
                    if (account != null) return account;
                }
            }
            return null;
        }
    }
}