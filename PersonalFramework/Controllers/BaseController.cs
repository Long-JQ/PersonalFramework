using Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace PersonalFramework.Controllers
{
    public class BaseController<T> : Controller where T : BaseEntity, new()
    {
        DataContext context = new DataContext();


        public string Delete(string id)
        {
            if (id == null)
            {
                return "false";
            }

            var entity = context.Set<T>().Find(id);
            if (entity == null)
            {
                return "false";
            }
            try
            {
                context.Set<T>().Remove(entity);
                context.SaveChangesAsync();
                return "true";
            }
            catch (DbUpdateException /* ex */)
            {
                return "false";
            }
        }

        //[HttpPost]
        //public ActionResult Edit(FormCollection fc)
        //{
        //    try
        //    {
        //        int id = Convert.ToInt32(fc["ID"]);
        //        var entity = context.Set<T>().Find(id);
        //        var onUser = entity.ToModel<T>();
        //        TryUpdateModel(entity, fc.AllKeys);
        //        if (entity.ID > 0)
        //        {
                    
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}
    }
}
