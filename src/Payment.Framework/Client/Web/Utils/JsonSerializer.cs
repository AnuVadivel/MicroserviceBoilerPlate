using System.IO;
using Flurl.Http.Configuration;
using Payment.Framework.Shared.Util;

namespace Payment.Framework.Client.Web.Utils
{
    public class JsonSerializer : ISerializer
    {
        public T Deserialize<T>(string s) => 
            SerializeUtility.Deserialize<T>(s);

        public T Deserialize<T>(Stream stream) => 
            SerializeUtility.Deserialize<T>(stream);

        public string Serialize(object obj) => 
            SerializeUtility.Serialize(obj);
    }
}