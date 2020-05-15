using Model;
using PagedList;
using PersonalFramework.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace PersonalFramework.Controllers
{
    public class RoleController : BaseController<Role>
    {
        DataContext context = new DataContext();
        [System.ComponentModel.DescriptionAttribute("角色列表页")]
        public ActionResult Index()
        {
            return View();
        }
        [System.ComponentModel.DescriptionAttribute("角色编辑页")]
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
                var entity = new Role();
                entity.ID = "";
                return View(entity);
            }
        }
        [System.ComponentModel.DescriptionAttribute("获取树形图数据")]
        public JsonResult GetTree(string id)
        {
            var result = UserHelper.GetTreeData(id);
            return Json(result);
        }

        [System.ComponentModel.DescriptionAttribute("配置权限页")]
        public ActionResult Auth(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var entity = GetEntity(id.ToString());
                return View(entity);
            }
            else
            {
                return RedirectToAction("index");
            }
        }

        
        /// <summary>
        /// 通用编辑与新增
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        public string AuthSubmit(string treeData,string id)
        {
            var a = treeData.ToObject<List<TreeData>>();
            if (ModelState.IsValid)
            {
                try
                {
                    var authID = "";
                    foreach (var item in a)
                    {
                        foreach (var item2 in item.children)
                        {
                            authID += item2.id + ";";
                        }
                    }

                    var entity = context.Roles.Find(id);
                    entity.AuthorityID = authID;
                    context.SaveChanges();
                    ReturnData result = new ReturnData(200, "编辑成功");
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
