using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace Service.Tool
{
    public class LogHelper
    {
        public static void WriteErrorLog(Exception ex)
        {
            try
            {
                if (ex != null)
                {

                    string content = "类型：错误代码\r\n";
                    content += "时间：" + DateTime.Now.ToString() + "\r\n";
                    content += "来源：" + ex.TargetSite.ReflectedType.ToString() + "." + ex.TargetSite.Name + "\r\n";
                    content += "内容：" + ex.Message + "\r\n";
                    //if (BaseConfigs.GetLogInDB == 1)
                    //{
                    //TSysLog log = new TSysLog();
                    //log.Content = content;
                    //log.Type = "系统日志";
                    //log.CreateTime = DateTime.Now;
                    //DataAccess.CreateSysLog().Add(log);
                    //}
                    //if (BaseConfigs.GetLogInFile == 1)
                    //{
                    Page page = new Page();
                    HttpServerUtility server = page.Server;
                    string dir = server.MapPath("/App_Data/Log/");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    string path = dir + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

                    StreamWriter FileWriter = new StreamWriter(path, true, System.Text.Encoding.UTF8); //创建日志文件
                    FileWriter.Write("---------------------------------------------------\r\n");
                    FileWriter.Write(content);
                    FileWriter.Close(); //关闭StreamWriter对象
                    //}
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static void WriteLog(string msg)
        {
            try
            {
                if (!string.IsNullOrEmpty(msg))
                {

                    string content = "类型：调试日志\r\n";
                    content += "时间：" + DateTime.Now.ToString() + "\r\n";
                    content += "内容：" + msg + "\r\n";

                    Page page = new Page();
                    HttpServerUtility server = page.Server;
                    string dir = server.MapPath("/App_Data/Log/");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    string path = dir + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

                    StreamWriter FileWriter = new StreamWriter(path, true, System.Text.Encoding.UTF8); //创建日志文件
                    FileWriter.Write("---------------------------------------------------\r\n");
                    FileWriter.Write(content);
                    FileWriter.Close(); //关闭StreamWriter对象
                    //}
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        //public static void WriteErrorLog(Exception ex)
        //{
        //    string content = "类型：错误代码\r\n";
        //    content += "时间：" + DateTime.Now.ToString() + "\r\n";
        //    content += "来源：" + ex.TargetSite.ReflectedType.ToString() + "." + ex.TargetSite.Name + "\r\n";
        //    content += "内容：" + ex.Message + "\r\n";

        //    TSysLog log = new TSysLog();
        //    log.Content = content;
        //    log.Type = "系统日志";
        //    log.CreateTime = DateTime.Now;
        //    DataAccess.CreateSysLog().Add(log);
        //}

        public static void WriteErrorLog(string errsrc, Exception ex)
        {
            try
            {
                if (ex != null)
                {
                    //if (ex.Source != "JN.Web")
                    //{
                    string content = (!string.IsNullOrEmpty(errsrc) ? "来自页面：" + errsrc : "") + "\r\n";
                    content += "发生时间：" + DateTime.Now.ToString() + "\r\n";
                    content += "异常对像：" + ex.TargetSite.ReflectedType.ToString() + "." + ex.TargetSite.Name + "\r\n";
                    content += "错误追踪：" + ex.StackTrace + "\r\n";
                    content += "错误提示：" + ex.Message + "\r\n";
                    if (ex.InnerException != null && ex.InnerException.InnerException != null)
                        content += ex.InnerException.InnerException.Message + "\r\n";
                    //if (BaseConfigs.GetLogInDB == 1)
                    //{
                    //TSysLog log = new TSysLog();
                    //log.Content = content;
                    //log.Type = "系统日志";
                    //log.CreateTime = DateTime.Now;
                    //DataAccess.CreateSysLog().Add(log);
                    //}
                    //if (BaseConfigs.GetLogInFile == 1)
                    //{
                    Page page = new Page();
                    HttpServerUtility server = page.Server;
                    string dir = server.MapPath("/App_Data/Log/");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    string path = dir + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

                    StreamWriter FileWriter = new StreamWriter(path, true, System.Text.Encoding.UTF8); //创建日志文件
                    FileWriter.Write("---------------------------------------------------\r\n");
                    FileWriter.Write(content);
                    FileWriter.Close(); //关闭StreamWriter对象
                                        //}
                                        //}
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static string GetMethodInfo()
        {
            string str = "";
            //取得当前方法命名空间
            str += "命名空间名:" + System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "\n";
            //取得当前方法类全名 包括命名空间
            str += "类名:" + System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + "\n";
            //取得当前方法名
            str += "方法名:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\n";
            str += "\n";

            //StackTrace ss = new StackTrace(true);
            //MethodBase mb = ss.GetFrame(1).GetMethod();
            ////取得父方法命名空间
            //str += mb.DeclaringType.Namespace + "\n";
            ////取得父方法类名
            //str += mb.DeclaringType.Name + "\n";
            ////取得父方法类全名
            //str += mb.DeclaringType.FullName + "\n";
            ////取得父方法名
            //str += mb.Name + "\n";
            return str;
        }

        public static void WritePlanLog(string logstr)
        {
            try
            {
                if (!string.IsNullOrEmpty(logstr))
                {

                    string content = logstr;

                    Page page = new Page();
                    HttpServerUtility server = page.Server;
                    string dir = server.MapPath("/TimingPlan/Log/");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    string path = dir + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

                    StreamWriter FileWriter = new StreamWriter(path, true, System.Text.Encoding.UTF8); //创建日志文件
                    FileWriter.Write("---------------------------------------------------\r\n");
                    FileWriter.Write(content);
                    FileWriter.Close(); //关闭StreamWriter对象
                    //}
                }
            }
            catch (Exception e)
            {
                throw;
            }
            //var log =MvcCore.
            //TSysLog log = new TSysLog();
            //log.Content = logstr;
            //log.Type = "作业日志";
            //log.CreateTime = DateTime.Now;
            //DataAccess.CreateSysLog().Add(log);
        }

        public static void WriteErrorTimingPlanLog(string errsrc, Exception ex)
        {
            try
            {
                if (ex != null)
                {
                    string content = (!string.IsNullOrEmpty(errsrc) ? "来自页面：" + errsrc : "") + "\r\n";
                    content += "发生时间：" + DateTime.Now.ToString() + "\r\n";
                    content += "异常对像：" + ex.TargetSite.ReflectedType.ToString() + "." + ex.TargetSite.Name + "\r\n";
                    content += "错误追踪：" + ex.StackTrace + "\r\n";
                    content += "错误提示：" + ex.Message + "\r\n";
                    if (ex.InnerException != null && ex.InnerException.InnerException != null)
                        content += ex.InnerException.InnerException.Message + "\r\n";
                    //if (BaseConfigs.GetLogInDB == 1)
                    //{
                    //TSysLog log = new TSysLog();
                    //log.Content = content;
                    //log.Type = "系统日志";
                    //log.CreateTime = DateTime.Now;
                    //DataAccess.CreateSysLog().Add(log);
                    //}
                    //if (BaseConfigs.GetLogInFile == 1)
                    //{
                    Page page = new Page();
                    HttpServerUtility server = page.Server;
                    string dir = server.MapPath("/TimingPlan/Log/");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    string path = dir + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

                    StreamWriter FileWriter = new StreamWriter(path, true, System.Text.Encoding.UTF8); //创建日志文件
                    FileWriter.Write("---------------------------------------------------\r\n");
                    FileWriter.Write(content);
                    FileWriter.Close(); //关闭StreamWriter对象
                                        //}
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public static void WritePlanLog1(Exception ex)
        {
            try
            {
                if (ex != null)
                {

                    string content = "类型：错误代码\r\n";
                    content += "时间：" + DateTime.Now.ToString() + "\r\n";
                    content += "来源：" + ex.TargetSite.ReflectedType.ToString() + "." + ex.TargetSite.Name + "\r\n";
                    content += "内容：" + ex.Message + "\r\n";
                    //if (BaseConfigs.GetLogInDB == 1)
                    //{
                    //TSysLog log = new TSysLog();
                    //log.Content = content;
                    //log.Type = "系统日志";
                    //log.CreateTime = DateTime.Now;
                    //DataAccess.CreateSysLog().Add(log);
                    //}
                    //if (BaseConfigs.GetLogInFile == 1)
                    //{
                    //Page page = new Page();
                    //HttpServerUtility server = page.Server;
                    string dir = AppDomain.CurrentDomain.BaseDirectory + "/TimingPlan/Log/";



                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    string path = dir + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

                    StreamWriter FileWriter = new StreamWriter(path, true, System.Text.Encoding.UTF8); //创建日志文件
                    FileWriter.Write("---------------------------------------------------\r\n");
                    FileWriter.Write(content);
                    FileWriter.Close(); //关闭StreamWriter对象
                    //}
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}
