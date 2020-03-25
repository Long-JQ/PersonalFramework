using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PersonalFramework.Controllers
{
    public static class JsonExtensions
    {
        public static string ToJson<T>(this T obj)
        {
            var time = new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            return JsonConvert.SerializeObject(obj, time);
        }
        public static string ToJson(this object obj)
        {
            var time = new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            return JsonConvert.SerializeObject(obj, time);
        }
        public static T ToObject<T>(this string data)
        {
            var settings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            return JsonConvert.DeserializeObject<T>(data, settings);
        }
    }
}
