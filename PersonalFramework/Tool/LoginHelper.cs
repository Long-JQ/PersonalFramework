using Model;
using PersonalFramework.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PersonalFramework.Tool
{
    public class LoginHelper
    {
        static User _loginuser = null;
        public LoginHelper()
        {

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
                
                var EncryptToken = LoginHelper.Login(account.Token);
                HttpContext.Current.Session[EncryptToken] = account;
                return EncryptToken;
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
                Tool.IPHelper.GetClientIp()

                );

            // 加密验证票据
            string strTicket = FormsAuthentication.Encrypt(ticket);

            // 使用新userdata保存cookie
            HttpCookie cookie = new HttpCookie(FormsCookieName, strTicket);
            //cookie.Expires = DateTime.Now.AddMinutes(30);
            cookie.Path = "/";

            HttpContext.Current.Response.Cookies.Add(cookie);
            return strTicket;
            #endregion
        }
        private static string FormsCookieName { get { return FormsAuthentication.FormsCookieName + "admin"; } }
        //获取当前会员登录对象
        /// <summary>
        /// 获取当前会员登录对象
        /// <para>当没登陆或者登录信息不符时，这里返回 null </para>
        /// </summary>
        /// <returns></returns>
        public static User GetUser(string token)
        {
            User entity = HttpContext.Current.Session[token] as User;
            FormsAuthenticationTicket ticket = null;
            if (entity != null)
            {
                ticket = FormsAuthentication.Decrypt(token);
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    FormsIdentity id = new FormsIdentity(ticket);
                    GenericPrincipal principal = new GenericPrincipal(id, new string[] { ticket.UserData });
                    HttpContext.Current.User = principal;//存到HttpContext.User中
                }
            }
            else
            {
                return null;
            }
            //客户端凭证验证不通过，要求重新登录
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                return null;
            
            //var ticket = ((System.Web.Security.FormsIdentity)HttpContext.Current.User.Identity).Ticket;
            //客户端IP不一样，要求重新登录

            //if (ticket.UserData != Tool.IPHelper.GetClientIp())
            //    return null; 
            
            return entity;
            
        }

        //登出
        /// <summary>
        /// 登出
        /// </summary>
        public static void UserLogout(string token)
        {
            if (GetUser(token) !=null)
            {
                //获取会员ID
                var id = HttpContext.Current.User.Identity.Name;
                FormsAuthentication.SignOut();
                _loginuser = null;
                RemoveUser(id);
                HttpContext.Current.Session.Remove(token);
            }
        }
        //移除指定会员ID的登录缓存
        /// <summary>
        /// 移除指定会员ID的登录缓存
        /// </summary>
        /// <param name="ID"></param>
        public static void RemoveUser(string ID)
        {
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddMinutes(-30);
            cookie.Path = "/";
            HttpContext.Current.Response.Cookies.Add(cookie);
            FormsAuthentication.SignOut();
        }
    }
}