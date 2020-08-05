using Model;
using PagedList;
using PersonalFramework.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace PersonalFramework.Controllers
{
    [System.ComponentModel.DescriptionAttribute("文章")]
    public class ArticleController : BaseController<Article>
    {
        DataContext context = new DataContext();
        [System.ComponentModel.DescriptionAttribute("文章列表页")]
        public ActionResult Index()
        {
            return View();
        }
        [System.ComponentModel.DescriptionAttribute("文章编辑页")]
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
                var entity = new Article();
                entity.ID = "";
                return View(entity);
            }
        }

        [System.ComponentModel.DescriptionAttribute("文章编辑提交")]
        [ValidateInput(false)]
        [HttpPost]
        public new string Edit(FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = context.Set<Article>().Find(fc["ID"]);

                    if (string.IsNullOrEmpty(fc["ID"]) && entity == null)
                    {
                        fc.Remove("ID");
                        entity = new Article();

                        entity.Author = AdminLoginHelper.CurrentUser().ID;

                        TryUpdateModel(entity, fc);
                        context.Set<Article>().Add(entity);
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


        [System.ComponentModel.DescriptionAttribute("文章图片上传")]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                file = Request.Files[0];
                HttpPostedFileBase imgFile = Request.Files["imgFile"];
                string oldLogo = "/Upload/";

                string img = Service.UploadPic.MvcUpload(file, new string[] { ".png", ".gif", ".jpg" }, 1, System.Web.HttpContext.Current.Server.MapPath(oldLogo));
                var imgurl = ".." + oldLogo + img;
                //ckeditor5图片返回格式
                return Json(new { uploaded = "1", fileName = img, url = imgurl }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new { src = "" }, code = 500, msg = "上传失败" }, JsonRequestBehavior.DenyGet);
            }
        }
    }
}
