using Model;
using PagedList;
using PersonalFramework.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace PersonalFramework.Controllers
{
    [System.ComponentModel.DescriptionAttribute("管理员")]
    public class AdminController : BaseController<Admin>
    {
        DataContext context = new DataContext();
        [System.ComponentModel.DescriptionAttribute("管理员列表页")]
        public ActionResult Index()
        {
            return View();
        }
        [System.ComponentModel.DescriptionAttribute("管理员编辑页")]
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
                var entity = new Admin();
                entity.ID = "";
                return View(entity);
            }
        }
        [System.ComponentModel.DescriptionAttribute("管理员编辑提交")]
        [HttpPost]
        public new string Edit(FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = context.Set<Admin>().Find(fc["ID"]);

                    if (string.IsNullOrEmpty(fc["ID"]) && entity == null)
                    {
                        fc.Remove("ID");
                        entity = new Admin();
                        TryUpdateModel(entity, fc);
                        entity.RoleName = context.Roles.Find(entity.RoleID).RoleName;
                        entity.Salt = PersonalFramework.Service.DeCrypt.SetSalt();
                        entity.Password = PersonalFramework.Service.DeCrypt.SetPassWord(entity.Password, entity.Salt);
                        context.Set<Admin>().Add(entity);
                        context.SaveChanges();
                        ReturnData result = new ReturnData(200, "编辑成功");
                        return result.ToJson();
                    }
                    else
                    {
                        TryUpdateModel(entity, fc);
                        entity.RoleName = context.Roles.Find(entity.RoleID).RoleName;
                        context.Entry(entity).Property(m => m.Salt).IsModified = false;
                        if (entity.Password == "")
                        {
                            context.Entry(entity).Property(m => m.Password).IsModified = false;
                        }
                        else
                        {
                            entity.Password = PersonalFramework.Service.DeCrypt.SetPassWord(entity.Password, entity.Salt);
                        }
                        context.SaveChanges();
                        ReturnData result = new ReturnData(200, "编辑成功");
                        return result.ToJson();
                    }

                }
                catch (DbEntityValidationException ex)
                {
                    ReturnData result = new ReturnData(500, ex.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage);
                    return result.ToJson();
                }
                catch (Exception ex)
                {
                    ReturnData result = new ReturnData(500, ex.Message);
                    return result.ToJson();
                }
            }
            else
            {
                ReturnData result = new ReturnData(500, "false");
                return result.ToJson();
            }
        }

    }
}
