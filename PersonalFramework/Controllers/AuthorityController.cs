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
    [System.ComponentModel.DescriptionAttribute("权限")]
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
        [System.ComponentModel.DescriptionAttribute("测试")]
        public string TestMethod()
        {
            DateTime dateTime = DateTime.Now;
            List<Test> tList = new List<Test>();
            Random random = new Random();
            //tList = context.Tests.Take(10000).ToList();
            for (int i = 0; i < 200000; i++)
            {
                Model.Test t = new Test();
                t.Name = random.Next(100000, 999999).ToString();
                tList.Add(t);
            }
            //foreach (var item in tList)
            //{
            //    item.Name = random.Next(100000, 999999).ToString();
            //}
            var endTime = DateTime.Now;
            var time = endTime - dateTime;

            dateTime = DateTime.Now;
            PersonalFramework.Context.DBHelper dBHelper = new Context.DBHelper();
            dBHelper.BulkInsert(tList,null, "Tests");
            //context.Tests.AddRange(tList);
            context.SaveChanges();
            endTime = DateTime.Now;
            var time2 = endTime - dateTime;
            return "";
        }
        [System.ComponentModel.DescriptionAttribute("权限扫描")]
        public string Scan()
        {
            DataContext context = new DataContext();
            List<Authority> authorities = new List<Authority>();
            List<Type> controllerTypes = new List<Type>();
            var oldAuthList = context.Authorities.ToList();
            string[] AuthorityController = { "ArticleClass", "Article", "Authority", "Notice", "Role", "Admin" };

            //加载程序集
            var assembly = System.Reflection.Assembly.Load("PersonalFramework");
            controllerTypes.AddRange(assembly.GetTypes().Where(type => typeof(IController).IsAssignableFrom(type) && type.Name != "BaseController`1"));

            //创建动态字符串，拼接json数据    注：现在json类型传递数据比较流行，比xml简洁
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("[");

            //遍历控制器类
            foreach (var controller in controllerTypes)
            {
                Authority authority = new Authority();
                authority.ParentID = "0";
                authority.ParentName = "0";
                authority.Action = controller.Name.Substring(0,controller.Name.Length-10).ToLower();
                authority.ActionDesc = (controller.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute) == null ? "" : (controller.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute).Description;

                if (!AuthorityController.Contains(controller.Name.Substring(0, controller.Name.Length - 10)))
                {
                    continue;
                }
                if (oldAuthList.Where(x => x.ParentName == authority.ParentName && x.Action == authority.Action).Count() == 0)
                {
                    authorities.Add(authority);
                }

                //获取控制器下所有返回类型为ActionResult的方法，对MVC的权限控制只要限制所以的前后台交互请求就行，统一为ActionResult
                var actions = controller.GetMethods().Where(method => (method.ReturnType.Name == "JsonResult" || method.ReturnType.Name == "String"|| method.ReturnType.Name == "ActionResult") && (method.DeclaringType.Name == controller.Name|| method.DeclaringType.Name.Contains("BaseController")));
                foreach (var action in actions)
                {
                    Authority authority2 = new Authority();
                    authority2.ParentID = authority.ID;
                    authority2.ParentName = authority.Action;
                    authority2.Action = action.Name.ToLower();
                    authority2.Type = action.ReturnType.Name;
                    authority2.ActionDesc = (action.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute) == null ? "" : (action.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute).Description;

                    if (oldAuthList.Where(x => x.ParentName == authority2.ParentName && x.Action == authority2.Action).Count() == 0)
                    {
                        authorities.Add(authority2);
                    }
                    
                }
            }
            context.Authorities.AddRange(authorities);
            context.SaveChanges();
            ReturnData result = new ReturnData(200, "");
            return result.ToJson();
        }

    }
}
