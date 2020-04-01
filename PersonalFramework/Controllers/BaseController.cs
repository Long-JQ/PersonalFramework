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
using PersonalFramework.Tool;

namespace PersonalFramework.Controllers
{
    public class BaseController<T> : Controller where T : BaseEntity, new()
    {
        DataContext context = new DataContext();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);


            //校验用户是否已经登录
            var model = PersonalFramework.Tool.LoginHelper.CurrentUser();
            if (model != null)
            {
                //Umodel = MvcCore.Unity.Get<JN.Data.Service.IUserService>().Single(model.ID);

                //string controllerName = filterContext.RouteData.Values["controller"].ToString().ToLower();
                //string actionName = filterContext.RouteData.Values["action"].ToString().ToLower();

                //string[] needactivecontroller = { "trade", "cfb", "mail" };
                //string[] needactiveaction = { "applytransfer" };

                //if (!Umodel.IsActivation && (needactivecontroller.Contains(controllerName) || needactiveaction.Contains(actionName)))
                //{
                //    Response.Redirect("/AppCenter/User/doPass");
                //    filterContext.Result = new EmptyResult();
                //}
            }
            else
            {
                string controllerName = filterContext.RouteData.Values["controller"].ToString().ToLower();
                if (controllerName == "icomanage") { Response.Redirect("/ico/sign_in"); }
                else
                {
                    Response.Redirect("/Login/index");
                    filterContext.Result = new EmptyResult();
                }

            }
        }
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
                ReturnData result = new ReturnData(500,"ID不能为空");
                return result.ToJson();
            }

            var entity = context.Set<T>().Find(id);
            if (entity == null)
            {
                ReturnData result = new ReturnData(500, "未找到数据");
                return result.ToJson();
            }
            try
            {
                context.Set<T>().Remove(entity);
                context.SaveChangesAsync();
                ReturnData result = new ReturnData(0,"删除成功");
                return result.ToJson();
            }
            catch (Exception ex)
            {
                ReturnData result = new ReturnData(500, ex.Message);
                return result.ToJson();
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
        public T GetEntity(string id)
        {
            if (id == null)
            {
                return null;
            }
            var entity = context.Set<T>().Find(id);
            if (entity == null)
            {
                return null;
            }

            return entity;
        }
        public string List(Pagination pagination)
        {
            try
            {
                var entityList = new List<T>();
                entityList = context.Set<T>().ToList();

                var resultList = entityList.ToPagedList(pagination.page, pagination.limit).OrderByDescending(x => x.CreateTime);
                ReturnData result = new ReturnData(0, "", entityList.Count, resultList, pagination.page);

                return result.ToJson();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Edit(FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = context.Set<T>().Find(fc["ID"]);
                    
                    if (string.IsNullOrEmpty(fc["ID"])&& entity == null)
                    {
                        fc.Remove("ID");
                        entity = new T();
                        TryUpdateModel(entity, fc);
                        context.Set<T>().Add(entity);
                        context.SaveChanges();
                        return Json(new { data = "", Status = 200 }, JsonRequestBehavior.DenyGet);
                    }
                    else
                    {
                        TryUpdateModel(entity, fc);
                        context.SaveChanges();
                        return Json(new { data = "", Status = 200 }, JsonRequestBehavior.DenyGet);
                    }
                    
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return Json(new { data = "", Status = 500, Message = ex.Message }, JsonRequestBehavior.DenyGet);
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
