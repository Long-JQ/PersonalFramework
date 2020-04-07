﻿using Model;
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
    public class ArticleClassController : BaseController<ArticleClass>
    {
        DataContext context = new DataContext();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var entity = Get(id.ToString());
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
