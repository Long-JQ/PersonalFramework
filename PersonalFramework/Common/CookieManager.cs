using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PersonalFramework.Common
{
    public class CookieManager: Controller
    {
        
        public CookieManager()
        {

        }
        public static string Generate(string signature)
        {
            if (string.IsNullOrWhiteSpace(signature))
                return "";

            var timestamp = DateTime.Now.ToString("yyyyMMddHHmm");

            var arr = new string[] { signature,".", timestamp };
            //Array.Sort(arr);

            var str = string.Join("", arr);
            return Base64Encode(str);
        }
        public static string TokenToSignature(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return "";

            //var means = 14;// (timestamp + nonce) lenght = 14
            var clearText = Base64Decode(token);
            var str = clearText.Split('.')[0];

            return str;
        }
        public static string Base64Encode(string content)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            return Convert.ToBase64String(bytes);
        }
        public static string Base64Decode(string content)
        {
            byte[] bytes = Convert.FromBase64String(content);
            return Encoding.UTF8.GetString(bytes);
        }
        /// <summary>
        /// 设置本地cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>  
        /// <param name="minutes">过期时长，单位：天</param>      
        public void SetCookies(string key, string value, int minutes = 30)
        {
            SetCookies(key, value, minutes);
            //HttpContext.Response.Cookies.Append(key, value, new CookieOptions
            //{
            //    Expires = DateTime.Now.AddDays(minutes)
            //});
        }
        /// <summary>
        /// 删除指定的cookie
        /// </summary>
        /// <param name="key">键</param>
        public void DeleteCookies(string key)
        {
            HttpContext.Response.Cookies.Remove(key);
        }

        /// <summary>
        /// 获取cookies
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回对应的值</returns>
        public string GetCookies(string key)
        {
            var cookie = HttpContext.Request.Cookies.Get(key);
            return cookie.Value;
        }
    }
}
