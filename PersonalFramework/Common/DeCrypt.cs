using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFramework.Common
{
    public class DeCrypt
    {
        /// <summary>
        /// 生成盐
        /// </summary>
        /// <returns></returns>
        public static byte[] SetSalt()
        {
            Random rnd = new Random();
            byte[] salt = new byte[6];
            rnd.NextBytes(salt);
            return salt;

        }


        /// <summary>
        /// 设置密码
        /// </summary>
        /// <param name="pwd">输入的密码</param>
        /// <param name="salt">盐</param>
        /// <returns></returns>
        public static byte[] SetPassWord(string pwd, byte[] salt)
        {
            byte[] Pwd;
            using (var sha = System.Security.Cryptography.SHA1.Create())
            {
                Pwd = sha.ComputeHash(sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pwd).Concat(salt).ToArray()));
            }
            return Pwd;
        }
        /// <summary>
        /// 密码验证
        /// </summary>
        /// <param name="pwd">明文，用户输入</param>
        /// <param name="PassWord">密文，储存在数据库</param>
        /// <param name="salt">盐</param>
        /// <returns></returns>
        public static bool VerifyPassWord(string pwd, byte[] PassWord, byte[] salt)
        {
            using (var sha = System.Security.Cryptography.SHA1.Create())
            {
                var data = sha.ComputeHash(sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pwd).Concat(salt).ToArray()));
                for (int i = 0; i < 20; i++)
                {
                    if (data[i] != PassWord[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
