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
    [System.ComponentModel.DescriptionAttribute("用户")]
    public class UserController : BaseController<User>
    {
        DataContext context = new DataContext();
        [System.ComponentModel.DescriptionAttribute("用户列表页")]
        public ActionResult Index()
        {
            return View();
        }
        [System.ComponentModel.DescriptionAttribute("用户编辑页")]
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
                var entity = new User();
                entity.ID = "";
                return View(entity);
            }
        }
        [System.ComponentModel.DescriptionAttribute("用户编辑提交")]
        public new ActionResult Edit(FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = context.Set<User>().Find(fc["ID"]);

                    if (string.IsNullOrEmpty(fc["ID"]) && entity == null)
                    {
                        fc.Remove("ID");
                        entity = new User();
                        TryUpdateModel(entity, fc);
                        entity.Salt = PersonalFramework.Service.DeCrypt.SetSalt();
                        entity.Password = PersonalFramework.Service.DeCrypt.SetPassWord(entity.Password, entity.Salt);
                        context.Set<User>().Add(entity);
                        context.SaveChanges();
                        return Json(new { data = "", Status = 200 }, JsonRequestBehavior.DenyGet);
                    }
                    else
                    {
                        TryUpdateModel(entity, fc);
                        entity.Password = PersonalFramework.Service.DeCrypt.SetPassWord(entity.Password, entity.Salt);
                        context.SaveChanges();
                        return Json(new { data = "", Status = 200 }, JsonRequestBehavior.DenyGet);
                    }

                }
                catch (Exception ex)
                {
                    return Json(new { data = "", Status = 500, Message = ex.Message }, JsonRequestBehavior.DenyGet);
                }
            }
            else
            {
                return Json(new { data = "", Status = 500, Message = "false" }, JsonRequestBehavior.DenyGet);
            }


        }

    }
}
