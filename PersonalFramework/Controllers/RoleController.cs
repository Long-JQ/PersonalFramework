using Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace PersonalFramework.Controllers
{
    public class RoleController : BaseController<Role>
    {
        DataContext context = new DataContext();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var entity = GetEntity(id.ToString());
                return View(entity);
            }
            else
            {
                var entity = new Role();
                entity.ID = "";
                return View(entity);
            }
        }
        public ActionResult a()
        {
            List<Type> controllerTypes = new List<Type>();

            //加载程序集
            var assembly = System.Reflection.Assembly.Load("PersonalFramework");
            controllerTypes.AddRange(assembly.GetTypes().Where(type => typeof(IController).IsAssignableFrom(type) && type.Name != "ErrorController"));

            //创建动态字符串，拼接json数据    注：现在json类型传递数据比较流行，比xml简洁
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("[");

            //遍历控制器类
            foreach (var controller in controllerTypes)
            {
                jsonBuilder.Append("{\"controllerName\":\"");
                jsonBuilder.Append(controller.Name);
                jsonBuilder.Append("\",\"controllerDesc\":\"");
                jsonBuilder.Append((controller.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute) == null ? "" : (controller.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute).Description);

                //获取对控制器的描述Description
                jsonBuilder.Append("\",\"action\":[");

                //获取控制器下所有返回类型为ActionResult的方法，对MVC的权限控制只要限制所以的前后台交互请求就行，统一为ActionResult
                var actions = controller.GetMethods().Where(method => method.ReturnType.Name == "ActionResult");
                foreach (var action in actions)
                {
                    jsonBuilder.Append("{\"actionName\":\"");
                    jsonBuilder.Append(action.Name);
                    jsonBuilder.Append("\",\"actionDesc\":\"");
                    jsonBuilder.Append((action.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute) == null ? "" : (action.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute).Description);    //获取对Action的描述
                    jsonBuilder.Append("\"},");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("]},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            return Content(jsonBuilder.ToString());
        }


    }
}
