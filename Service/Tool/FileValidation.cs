using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace PersonalFramework.Service
{
    public class FileValidation
    {
        public static bool IsAllowedExtension(HttpPostedFileBase fu, FileExtension[] fileEx)
        {
            int fileLen = fu.ContentLength;
            byte[] imgArray = new byte[fileLen];
            fu.InputStream.Read(imgArray, 0, fileLen);
            MemoryStream ms = new MemoryStream(imgArray);
            System.IO.BinaryReader br = new System.IO.BinaryReader(ms);
            string fileclass = "";
            byte buffer;
            try
            {
                buffer = br.ReadByte();
                fileclass = buffer.ToString();
                buffer = br.ReadByte();
                fileclass += buffer.ToString();
            }
            catch
            {
            }
            br.Close();
            ms.Close();
            //注意将文件流指针还原
            fu.InputStream.Position = 0;
            foreach (FileExtension fe in fileEx)
            {
                if (Int32.Parse(fileclass) == (int)fe)
                    return true;
            }
            return false;
        }



        /// <summary>
        /// 根据文件头判断上传的文件类型
        /// </summary>
        /// <param name="filePath">filePath是文件的完整路径 </param>
        /// <returns>返回true或false</returns>
        public static bool IsPicture(string filePath)
        {
            try
            {
                string fileClass;
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    BinaryReader reader = new BinaryReader(fs);
                    //string fileClass;
                    byte buffer;
                    buffer = reader.ReadByte();
                    fileClass = buffer.ToString();
                    buffer = reader.ReadByte();
                    fileClass += buffer.ToString();
                    reader.Close();
                    fs.Close();

                }
                if (fileClass == "255216" || fileClass == "7173" || fileClass == "13780" || fileClass == "6677")
                //255216是jpg;7173是gif;6677是BMP,13780是PNG;7790是exe,8297是rar 
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 判断文件是否为图片
        /// </summary>
        /// <param name="path">文件的完整路径</param>
        /// <returns>返回结果</returns>
        public static Boolean IsImage(string path)
        {
            try
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(path);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static Boolean IsImage2(Stream stream)
        {
            try
            {
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }




        /// <summary>
        /// 检查图片是否通过，返回状态0通过，-100一般， -200危险    ——181130 dong
        /// </summary>
        /// <param name="file">文件流</param>
        /// <param name="msg">提示</param>
        /// <returns></returns>
        public static int ImageCheck(HttpPostedFileBase file, ref string msg)
        {
            int status = 0;

            msg += file.FileName;

            //文件名正则判断  兼容中文及字母和常用字符
            if (!Regex.IsMatch(file.FileName.Replace(" ", ""), @"^[\u4e00-\u9fa5A-Za-z0-9_+@*#-.]+$"))
            {
                status = -110;
                msg += "/" + "-110含其他字符";
            }


            //文件名后缀判断
            bool EndStrIsPass = false;
            string EndStr = Path.GetExtension(file.FileName).ToUpper();  //获取文件后缀
            string[] allowExtension = { ".jpg", ".jpeg", ".bmp", ".png" };
            foreach (var item in allowExtension)
            {
                if (item.Equals(EndStr, StringComparison.OrdinalIgnoreCase))  //忽略大小写比较
                {
                    EndStrIsPass = true;
                    break;
                }
            }
            if (!EndStrIsPass)
            {
                status = -120;
                msg += "/" + "-120不符后缀:" + EndStr;
            }


            //文件名包含判断
            bool EndStrIsHave = false;
            string[] dangerStr = { ".php", ".php3", ".php5", ".phtml", ".asp", ".aspx", ".ascx", ".jsp", ".cfm", ".cfc", ".pl", "pl", "bat", ".exe", ".dll", ".reg", ".cgi" };
            foreach (var item in dangerStr)
            {
                if (file.FileName.ToLower().Contains(item.ToLower()))  //小写比较
                {
                    EndStrIsHave = true;
                    break;
                }
            }
            if (EndStrIsHave)
            {
                status = -210;
                msg += "/" + "-210含危险字符:" + EndStr;
            }


            return status;
        }


        /// <summary>
        /// 检查图片是否通过，返回状态0通过，-100一般， -200危险     ——181130 dong
        /// </summary>
        /// <param name="fileName">实际路径</param>
        /// <param name="fileStr">上传名称</param>
        /// <param name="msg">提示</param>
        /// <returns></returns>
        public static int ImageCheck2(string fileName, string fileStr, ref string msg)
        {
            int imgStatus = 0;
            msg += fileStr;

            //if (!JN.Services.Tool.FileValidation.IsImage(fileName))
            //{
            //    imgStatus = -101;
            //    msg += "/" + "-101不是图片";
            //}

            //if (!JN.Services.Tool.FileValidation.IsPicture(fileName))
            //{
            //    imgStatus = -102;
            //    msg += "/" + "-102文件头不符";
            //}


            //高级验证，图片上传后的操作，判断是否真的是图片
            StreamReader sr = new StreamReader(fileName, Encoding.Default);
            string strContent = sr.ReadToEnd();
            sr.Close();
            string str = "request|.getfolder|.createfolder|.deletefolder|.createdirectory|.deletedirectory|.saveas|wscript.shell|script.encode|server.|.createobject|execute|activexobject|language=|namespace=|</script>|<?php|<script";   //|catch|return|private|script|void
            foreach (string s in str.Split('|'))
                if (strContent.ToLower().IndexOf(s) != -1)
                {
                    imgStatus = -201;
                    msg += "/" + "-201内容包含" + s;
                    break;
                }

            if (imgStatus == -201)
            {
                //引用的函数外已经有删除
                //if (System.IO.File.Exists(fileName))  //是否存在
                //{
                //    System.IO.File.Delete(fileName);  //删除
                //}
            }

            return imgStatus;
        }
    }
    public enum FileExtension
    {
        GIF = 7173,
        JPG = 255216,
        BMP = 6677,
        PNG = 13780,
        DOC = 208207,
        DOCX = 8075,
        XLSX = 8075,
        JS = 239187,
        XLS = 208207,
        SWF = 6787,
        MID = 7784,
        RAR = 8297,
        ZIP = 8075,
        XML = 6063,
        TXT = 7067,
        MP3 = 7368,
        WMA = 4838,

        // 239187 aspx
        // 117115 cs
        // 119105 js
        // 210187 txt
        //255254 sql 		
        // 7790 exe dll,
        // 8297 rar
        // 6063 xml
        // 6033 html
    }
}
