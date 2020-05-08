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
    public class NoticeController : BaseController<Notice>
    {
        DataContext context = new DataContext();
        [System.ComponentModel.DescriptionAttribute("公告列表页")]
        public ActionResult Index()
        {
            return View();
        }
        [System.ComponentModel.DescriptionAttribute("公告编辑页")]
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
                var entity = new Notice();
                entity.ID = "";
                return View(entity);
            }
        }
        [ValidateInput(false)]
        public override string Edit(FormCollection fc)
        {
            var result = base.Edit(fc);
            return result;
        }

    }
}
