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
    [System.ComponentModel.DescriptionAttribute("文章分类")]
    public class ArticleClassController : BaseController<ArticleClass>
    {
        DataContext context = new DataContext();
        [System.ComponentModel.DescriptionAttribute("文章分类列表页")]
        public ActionResult Index()
        {
            return View();
        }
        [System.ComponentModel.DescriptionAttribute("文章分类编辑页")]
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
                var entity = new ArticleClass();
                entity.ID = "";
                return View(entity);
            }
        }
        
        
    }
}
