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
using PersonalFramework.Service;
using System.Web;

namespace PersonalFramework.Controllers
{
    public class BaseController<T> : Controller where T : BaseEntity, new()
    {
        DataContext context = new DataContext();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            
            //校验用户是否已经登录
            var model = PersonalFramework.Service.UserLoginHelper.CurrentUser();
            if (model != null)
            {
                //string controllerName = filterContext.RouteData.Values["controller"].ToString().ToLower();
                //string actionName = filterContext.RouteData.Values["action"].ToString().ToLower();

                //if (context.Authorities.Where(x => x.ControllerName == controllerName && x.ActionName == actionName).Count() > 0)
                //{
                //    var adminVerify = context.Authorities.Single(x => x.ControllerName == controllerName && x.ActionName == actionName);

                //    var role = context.Roles.Where(x=>x.ID == model.RoleID).Single();
                //    if (role != null)
                //    {
                //        if (!(role.AuthorityID + ",").Contains("," + adminVerify.ID + ","))
                //        {
                //            Response.Redirect(Url.Action("NoAuthority", "Home"));
                //            filterContext.Result = new EmptyResult();
                //        }
                //    }
                //    else
                //    {
                //        Response.Redirect(Url.Action("NoAuthority", "Home"));
                //        filterContext.Result = new EmptyResult();
                //    }
                //}
                //else
                //{
                //    Response.Redirect(Url.Action("NoAuthority", "Home"));
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
        /// <summary>
        /// 通用新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string Add(T entity)
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();
            return "true";
        }
        /// <summary>
        /// 通用删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 通用实体检查
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 通用单个查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 通用列表集合查询
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 通用编辑与新增
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        public virtual string Edit(FormCollection fc)
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
                        ReturnData result = new ReturnData(200, "编辑成功");
                        return result.ToJson();
                    }
                    else
                    {
                        TryUpdateModel(entity, fc);
                        context.SaveChanges();
                        ReturnData result = new ReturnData(200, "编辑成功");
                        return result.ToJson();
                    }
                    
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ReturnData result = new ReturnData(500, ex.Message);
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
        public bool EntityExists(string id)
        {
            return context.Set<T>().Any(e => e.ID == id);
        }
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                HttpPostedFileBase imgFile = Request.Files["imgFile"];
                string oldLogo = "/Upload/";

                string img = UploadPic.MvcUpload(file, new string[] { ".png", ".gif", ".jpg" }, 1, System.Web.HttpContext.Current.Server.MapPath(oldLogo));
                img = ".." + oldLogo + img;
                return Json(new { data = new { src = img }, code = 0, msg = "成功" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new { src = "" }, code = 500, msg = "上传失败" }, JsonRequestBehavior.DenyGet);
            }
        }
        /// <summary>
        /// 引用类型实体复制
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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
