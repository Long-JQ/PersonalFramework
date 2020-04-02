using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFramework.Service
{
    public static class DeCrypt
    {
        /// <summary>
        /// 生成盐
        /// </summary>
        /// <returns></returns>
        public static string SetSalt()
        {
            Random rnd = new Random();
            byte[] salt = new byte[6];
            rnd.NextBytes(salt);
            return Convert.ToBase64String(salt);

        }


        /// <summary>
        /// 设置密码
        /// </summary>
        /// <param name="pwd">输入的密码</param>
        /// <param name="salt">盐</param>
        /// <returns></returns>
        public static string SetPassWord(string pwd, string salt)
        {
            byte[] Pwd;
            using (var sha = System.Security.Cryptography.SHA512.Create())
            {
                Pwd = sha.ComputeHash(sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pwd+salt).ToArray()));
            }
            return Convert.ToBase64String(Pwd);
        }
        /// <summary>
        /// 密码验证
        /// </summary>
        /// <param name="pwd">明文，用户输入</param>
        /// <param name="PassWord">密文，储存在数据库</param>
        /// <param name="salt">盐</param>
        /// <returns></returns>
        public static bool VerifyPassWord(string pwd, string PassWord, string salt)
        {
            using (var sha = System.Security.Cryptography.SHA512.Create())
            {
                var data = sha.ComputeHash(sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pwd + salt).ToArray()));
                var tmpPassWord = Convert.ToBase64String(data);
                if (tmpPassWord != PassWord)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public static string getToken(string signature)
        {
            if (string.IsNullOrWhiteSpace(signature))
                return "";

            var timestamp = DateTime.Now.ToString("yyyyMMddHHmm");

            var arr = new string[] { signature, ".", timestamp };
            //Array.Sort(arr);

            var str = string.Join("", arr);
            return Base64Encode(str);
        }
        public static string Base64Encode(string content)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            return Convert.ToBase64String(bytes);
        }
        public static string ToSHA512(this string content)
        {
            byte[] result;
            using (var sha = System.Security.Cryptography.SHA512.Create())
            {
                result = sha.ComputeHash(sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(content).ToArray()));
            }
            return Convert.ToBase64String(result);
        }
        public static string ToMD5(this string content)
        {
            byte[] result;
            using (var sha = System.Security.Cryptography.MD5.Create())
            {
                result = sha.ComputeHash(sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(content).ToArray()));
            }
            return Convert.ToBase64String(result);
        }
    }
}
