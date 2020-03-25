using Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace PersonalFramework.Controllers
{
    public class BaseController<T> : Controller where T : BaseEntity, new()
    {
        DataContext context = new DataContext();

        public string Add(T entity)
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();
            return "true";
        }
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
        
        public string Get(string id)
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

            return entity.ToJson();
        }
        public string List(Pagination pagination)
        {
            try
            {
                var entityList = new List<T>();
                entityList = context.Set<T>().ToList();

                var resultList = entityList.ToPagedList(pagination.page, pagination.limit).OrderByDescending(x => x.CreateTime);
                ReturnData result = new ReturnData(200, "success", entityList.Count, resultList.ToJson(), pagination.page);

                return result.ToJson();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string Edit(FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = context.Set<T>().Find(fc["ID"]);
                    if (!EntityExists(entity.ID))
                    {
                        return "false";
                    }
                    TryUpdateModel(entity, fc);
                    context.SaveChanges();
                    return "true";
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return ex.Message;
                }
            }
            return "false";
        }
        public bool EntityExists(string id)
        {
            return context.Set<T>().Any(e => e.ID == id);
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
