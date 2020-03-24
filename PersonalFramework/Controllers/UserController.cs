using Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Mvc;

namespace PersonalFramework.Controllers
{
    public class UserController : BaseController<User>
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
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                return View();
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult Edit(FormCollection fc)
        {
            return View();
        }
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

        public static object DeepCopyObject(object obj)
        {
            BinaryFormatter Formatter = new BinaryFormatter(null,
             new StreamingContext(StreamingContextStates.Clone));
            MemoryStream stream = new MemoryStream();
            Formatter.Serialize(stream, obj);
            stream.Position = 0;
            object clonedObj = Formatter.Deserialize(stream);
            stream.Close();
            return clonedObj;
        }
    }
}
