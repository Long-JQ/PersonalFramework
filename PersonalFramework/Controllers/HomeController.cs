using Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace PersonalFramework.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            
            return View();
        }

        public ActionResult List(int? page, int? limit)
        {
            var a = new User();
            a.RoleName = "1";
            a.UserName = "2";
            var list = new List<User>();
            list.Add(a);
            list.Add(a);
            list.Add(a);
            var listdata = list.ToPagedList(page ?? 1, limit ?? 1);//取数据
            return Json(new { data = list, code = 0, msg = "", count = list.Count() });
        }
    }
}
