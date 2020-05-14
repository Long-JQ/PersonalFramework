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
        [System.ComponentModel.DescriptionAttribute("角色列表页")]
        public ActionResult Index()
        {
            return View();
        }
        [System.ComponentModel.DescriptionAttribute("角色编辑页")]
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

        [System.ComponentModel.DescriptionAttribute("配置权限页")]
        public ActionResult Auth(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var entity = GetEntity(id.ToString());
                return View(entity);
            }
            else
            {
                return RedirectToAction("index");
            }
        }

        


    }
}
