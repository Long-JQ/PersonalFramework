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
using System.Data.Entity.Validation;

namespace PersonalFramework.Controllers
{
    [System.ComponentModel.DescriptionAttribute("通用类")]
    [ExceptionFilter]
    public class BaseController<T> : Controller where T : BaseEntity, new()
    {
        DataContext context = new DataContext();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            //校验用户是否已经登录
            var admin = PersonalFramework.Service.AdminLoginHelper.CurrentUser();
            var model = PersonalFramework.Service.UserLoginHelper.CurrentUser();
            if (admin != null || model != null)
            {
                if (admin == null && model != null)
                {
                    string controllerName = filterContext.RouteData.Values["controller"].ToString().ToLower();
                    string actionName = filterContext.RouteData.Values["action"].ToString().ToLower();
                    var AuthList = context.Authorities.ToList();

                    if (AuthList.Where(x => x.ParentName == controllerName && x.Action == actionName).Count() > 0)
                    {
                        var adminVerify = AuthList.Where(x => x.ParentName == controllerName && x.Action == actionName).FirstOrDefault();

                        var role = context.Roles.Where(x => x.ID == model.RoleID).Single();
                        if (role != null)
                        {
                            if (!(role.AuthorityID + ",").Contains("," + adminVerify.ID + ","))
                            {
                                Response.Redirect(Url.Action("NoAuthority", "Home"));
                                filterContext.Result = new EmptyResult();
                            }
                        }
                        else
                        {
                            Response.Redirect(Url.Action("NoAuthority", "Home"));
                            filterContext.Result = new EmptyResult();
                        }
                    }
                    else
                    {
                        Response.Redirect(Url.Action("NoAuthority", "Home"));
                        filterContext.Result = new EmptyResult();
                    }
                }
                
            }
            else if (admin == null || model == null)
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

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            string controllerName = filterContext.RouteData.Values["controller"].ToString().ToLower();
            string actionName = filterContext.RouteData.Values["action"].ToString().ToLower();
            var admin = PersonalFramework.Service.AdminLoginHelper.CurrentUser();

            //记录操作日志，写进操作日志中
            var log = new ActionLog();
            log.ActionContent = "";
            log.CreateTime = DateTime.Now;
            log.IP = IPHelper.GetClientIp();
            log.Location = controllerName + "/" + actionName;
            log.RequestData = Request.Form.ToString();
            log.Platform = "后台";
            log.Source = Request.HttpMethod;
            log.RequestUrl = Request.Url.AbsoluteUri;
            if (admin != null)
            {
                log.UID = admin.ID;
                log.UserName = admin.AdminName;
            }
            context.ActionLog.Add(log);
            //context.SaveChanges();
            
            if (Request.UrlReferrer != null)
            {
                ViewBag.FormUrl = Request.UrlReferrer.ToString();
            }
        }


        [System.ComponentModel.DescriptionAttribute("通用新增")]
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
        [System.ComponentModel.DescriptionAttribute("通用删除")]
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
        [System.ComponentModel.DescriptionAttribute("通用实体检查")]
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
        [System.ComponentModel.DescriptionAttribute("通用单个查询")]
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
        [System.ComponentModel.DescriptionAttribute("通用列表集合查询")]
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
                ReturnData result = new ReturnData(500, ex.Message);
                return result.ToJson();
            }
        }
        [System.ComponentModel.DescriptionAttribute("通用编辑与新增")]
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
