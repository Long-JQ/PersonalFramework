using Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace PersonalFramework.Controllers
{
    public class AdminController : BaseController<Admin>
    {
        DataContext context = new DataContext();
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                var entity = Get(id.ToString());
                return View(entity);
            }
            else
            {
                var entity = new User();
                entity.ID = "";
                return View(entity);
            }
        }
        //[HttpPost]
        //public ActionResult Edit(FormCollection fc)
        //{
        //    return View();
        //}
        public ActionResult List(int? page, int? limit)
        {
            var a = new User();
            var b = new User();
            a.RoleName = "1";
            a.UserName = "2";
            b = (User)DeepCopyObject(a);
            a.RoleName = "2";
            var list = new List<User>();
            list.Add(a);
            list.Add(a);
            list.Add(a);
            var listdata = list.ToPagedList(page ?? 1, limit ?? 1);//取数据
            return Json(new { data = list, code = 0, msg = "", count = list.Count() }, JsonRequestBehavior.AllowGet);
        }

        
    }
}
