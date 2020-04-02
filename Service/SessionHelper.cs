using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Mvc;

namespace PersonalFramework.Service
{
    public class SessionHelper<T>
    {
        public static bool SetSession(string key,T entity)
        {
            HttpContext.Current.Session[key] = entity;
            return true;
        }
        public T GetSession(string key)
        {
            var entity = (T)HttpContext.Current.Session[key];
            return entity;
        }
    }
}