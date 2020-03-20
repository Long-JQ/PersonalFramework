
using PersonalFramework.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PersonalFramework.Controllers
{
    public class ValuesController : ApiController
    {
        private string testdata;
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        public string Pa()
        {
            var p = "";
            var salt = DeCrypt.SetSalt();
            var reuslt = DeCrypt.SetPassWord("123", salt);
            var bb = System.Text.Encoding.UTF8.GetBytes("123");
            return reuslt;
        }
        [HttpPost]
        public int ge(int id)
        {
            return id;
        }
        [HttpPost]
        public string setData(string str)
        {
            testdata = str;
            return testdata;
        }
        public string getData()
        {
            return testdata;
        }
    }
}
