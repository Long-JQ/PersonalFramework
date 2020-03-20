using Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PersonalFramework.Controllers
{
    public class UserController : ApiController
    {
        // GET: api/User
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/User/5
        public string Get(int id)
        {
            return "value";
        }
        [HttpGet]
        public IHttpActionResult List(int? page, int? limit)
        {
            var a = new User();
            a.RoleName = "1";
            a.UserName = "2";
            var list = new List<User>();
            list.Add(a);
            list.Add(a);
            list.Add(a);
            var listdata = list.ToPagedList(page ?? 1, limit ?? 1);//取数据
            return Json(new { data = list, code = 0, msg = "", count = list.Count() });
        }

        // POST: api/User
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/User/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
        }
    }
}
