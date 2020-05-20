using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Helpers
{
    public static class CookieHelpers
    {
        public static void SetObjectAsJson(this IResponseCookies cookies, string key, object value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddDays(10);
            cookies.Append(key, JsonConvert.SerializeObject(value), option);
        }
        public static void RemoveCookie(this IResponseCookies cookies, string key)
        {
            cookies.Delete(key);
        }

        public static T GetObjectFromJson<T>(this IRequestCookieCollection cookie, string key)
        {
            var value = cookie[key];
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
