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
