using Model;
using PagedList;
using PersonalFramework.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Service
{
    public class DataHelper<T> : Controller where T : BaseEntity, new()
    {
        DataContext context = new DataContext();

        #region 新增
        /// <summary>
        /// 通用新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string Add(T entity)
        {
            try
            {
                context.Set<T>().Add(entity);
                context.SaveChanges();
                ReturnData result = new ReturnData(0, "添加成功");
                return result.ToJson();
            }
            catch (Exception ex)
            {
                ReturnData result = new ReturnData(500, ex.Message);
                return result.ToJson();
            }
        }
        #endregion

        #region 删除
        /// <summary>
        /// 通用删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Delete(string id)
        {
            if (id == null)
            {
                ReturnData result = new ReturnData(500, "ID不能为空");
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
                ReturnData result = new ReturnData(0, "删除成功");
                return result.ToJson();
            }
            catch (Exception ex)
            {
                ReturnData result = new ReturnData(500, ex.Message);
                return result.ToJson();
            }
        }
        #endregion

        #region 查询
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
                throw ex;
            }
        }
        #endregion

        #region 编辑
        /// <summary>
        /// 通用编辑与新增
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        public virtual string Edit(System.Web.Mvc.FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = context.Set<T>().Find(fc["ID"]);

                    if (string.IsNullOrEmpty(fc["ID"]) && entity == null)
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
        #endregion

        public bool EntityExists(string id)
        {
            return context.Set<T>().Any(e => e.ID == id);
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
    }
}
