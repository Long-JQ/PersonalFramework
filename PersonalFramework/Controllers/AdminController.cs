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
        
        
    }
}
