using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RuggedBooksUtilities
{
    public static class SessionExtension
    {
        public static void setObject(this ISession session, string key, Object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T getObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return (value == null) ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
