using Model;
using PagedList;
using PersonalFramework.Service;
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
    public class AuthorityController : BaseController<Authority>
    {
        DataContext context = new DataContext();
        [System.ComponentModel.DescriptionAttribute("权限列表页")]
        public ActionResult Index()
        {
            return View();
        }
        [System.ComponentModel.DescriptionAttribute("权限编辑页")]
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
                var entity = new Authority();
                entity.ID = "";
                return View(entity);
            }
        }
        public string scan()
        {
            DataContext context = new DataContext();
            List<Authority> authorities = new List<Authority>();
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
                Authority authority = new Authority();
                authority.ParentID = "0";
                authority.ParentName = "0";
                authority.Action = controller.Name;
                authority.ActionDesc = (controller.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute) == null ? "" : (controller.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute).Description;
                authorities.Add(authority);
                //获取控制器下所有返回类型为ActionResult的方法，对MVC的权限控制只要限制所以的前后台交互请求就行，统一为ActionResult
                var actions = controller.GetMethods().Where(method => method.ReturnType.Name == "ActionResult" && method.DeclaringType.Name == controller.Name);
                foreach (var action in actions)
                {
                    Authority authority2 = new Authority();
                    authority2.ParentID = authority.ID;
                    authority2.ParentName = authority.Action;
                    authority2.Action = action.Name;
                    authority2.ActionDesc = (action.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute) == null ? "" : (action.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute).Description;
                    authorities.Add(authority2);
                }
            }
            context.Authorities.AddRange(authorities);
            context.SaveChanges();
            ReturnData result = new ReturnData(200, "");
            return result.ToJson();
        }

    }
}
